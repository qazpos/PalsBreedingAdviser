using System.Text;
using System.Text.RegularExpressions;

namespace PalworldSaveDecoding
{
    public class GvasFileReader : BinaryReader
    {
        public GvasFileReader(Stream input, bool collectEnums = false) : base(input)
        {
            this.collectEnums = collectEnums;
        }


        public bool IsBaseStreamOnStart => BaseStream.Position == 0;
        public bool IsBaseStreamEnds => BaseStream.Position == BaseStream.Length;
        public ulong BytesLeft => (ulong)Math.Max(BaseStream.Length - BaseStream.Position, 0);
        public float WasReadPart => ((float)BaseStream.Position / BaseStream.Length);

        private const double doubleCompressCoefficient = 360d / 65536d;

        private static Regex enumRegex = new Regex(@"^E\S*::\S+", RegexOptions.IgnoreCase);
        private bool collectEnums;



        public byte[] ReadToEnd()
        {
            var result = new List<byte>();
            while (BytesLeft != 0) {
                if (BytesLeft > int.MaxValue)
                    result.AddRange(ReadBytes(int.MaxValue));
                else
                    result.AddRange(ReadBytes((int)BytesLeft));
            }
            return result.ToArray();
        }


        public float ReadFloat() => BitConverter.ToSingle(ReadBytes(4));

        public void Skip(int length) => ReadBytes(length);
        public void Skip(ulong length)
        {
            while (length > int.MaxValue) {
                Skip(int.MaxValue);
                length -= int.MaxValue;
            }
            Skip((int)length);
        }



        public Guid ReadGuid() => new Guid(ReadBytes(16));

        public Guid? ReadGuidOptional() => ReadByte() == 0 ? null : ReadGuid();



        public DateTime ReadDateTime() => new(ReadInt64(), DateTimeKind.Local);

        public DateTime ReadDateTimeProperty() => new(ReadInt64Property(), DateTimeKind.Local);

        public DateTime ReadDateTimeComplex(string fieldName)
        {
            var structName = ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Invalid DateTime struct parameter {structName}");

            var typeName = ReadString();
            var size = ReadUInt64();

            var result = ReadDateTime();

            structName = ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Invalid DateTime struct parameter {structName}");

            return result;
        }

        public DateTime ReadDateTimePropertyComplex(string fieldName)
        {
            var structName = ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Invalid DateTime struct parameter {structName}");

            var typeName = ReadString();
            var size = ReadUInt64();

            var result = ReadDateTimeProperty();

            structName = ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Invalid DateTime struct parameter {structName}");

            return result;
        }




        public override string ReadString() //Аналог fstring
        {
            var length = ReadInt32();
            if (length == 0)
                return "";

            string result;

            if (length < 0) {
                length = -length * 2;
                var b = ReadBytes(length);
                result = Encoding.Unicode.GetString(b, 0, length - 2);
            } else {
                var b = ReadBytes(length);
                result = Encoding.ASCII.GetString(b, 0, length - 1);
            }

            if (collectEnums && enumRegex.IsMatch(result))
                EnumCollector.AddValue(result);

            return result;
        }

        public string ReadStringProperty()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadString();
        }




        public T ReadEnum<T>()
            where T : struct
        {
            return ParseEnum<T>(ReadEnumAsString());
        }

        public T ReadEnumProperty<T>()
            where T : struct
        {
            return ParseEnum<T>(ReadEnumPropertyAsString());
        }

        public string ReadEnumAsString() => ReadString();

        public string ReadEnumPropertyAsString()
        {
            var enumType = ReadString();
            return ReadStringProperty();
        }

        private T ParseEnum<T>(string enumStr)
            where T : struct
        {
            enumStr = enumStr.Substring(enumStr.IndexOf("::") + 2);
            if (!Enum.TryParse<T>(enumStr, out T result))
                throw new InvalidDataException($"Unknown enum {typeof(T).Name} value {enumStr}");
            return result;
        }




        public Vector3D ReadVector3D()
            => new(ReadDouble(), ReadDouble(), ReadDouble());

        public Vector3L ReadVector3L()
            => new(ReadInt64(), ReadInt64(), ReadInt64());

        public QuaternionD ReadQuaternionD()
            => new(ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble());

        public Transform ReadTransform()
            => new Transform(ReadQuaternionD(), ReadVector3D(), ReadVector3D());

        public Vector3L ReadVector3LPropertyComplex()
        {
            var structName = ReadString();
            long x = 0, y = 0, z = 0;
            while (structName != "None") {
                var subTypeName = ReadString();
                var subSize = ReadUInt64();

                switch (structName) {
                    case "X":
                        x = ReadInt64Property(); break;
                    case "Y":
                        y = ReadInt64Property(); break;
                    case "Z":
                        z = ReadInt64Property(); break;
                    default:
                        throw new InvalidDataException($"Invalid Vector3Int64 struct parameter {structName}");
                }

                structName = ReadString();
            }

            return new(x, y, z);
        }





        public T[] ReadArray<T>(Func<T> readElement, bool isTStruct = false)
        {
            var count = ReadUInt32();
            var result = new T[count];
            if (isTStruct) {
                var propName = ReadString();
                var propType = ReadString();
                ReadUInt64();
                var typeName = ReadString();
                var guid = ReadGuid();
                Skip(1);
            }
            for (var i = 0; i < count; i++)
                result[i] = readElement();
            return result;
        }

        public T[] ReadArrayProperty<T>(Func<T> readElement, bool isTStruct = false)
        {
            var arrayType = ReadString();
            var optionalGuid = ReadGuidOptional();
            return ReadArray<T>(readElement, isTStruct);
        }

        public void SkipArrayProperty(ulong size)
        {
            var arrayType = ReadString();
            var optionalGuid = ReadGuidOptional();
            Skip(size);
        }

        public T[] ReadArrayPropertyComplex<T>(Func<T> readElement, string fieldName, bool isTStruct = false)
        {
            var structName = ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Invalid array struct parameter {structName}");
            var typeName = ReadString();
            var size = ReadUInt64();

            var result = ReadArrayProperty<T>(readElement, isTStruct);

            structName = ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Invalid array struct parameter {structName}");

            return result;
        }




        public List<T> ReadList<T>(Func<T> readElement, bool isTStruct = false)
        {
            return new List<T>(ReadArray<T>(readElement, isTStruct));
        }

        public List<T> ReadListProperty<T>(Func<T> readElement, bool isTStruct = false)
        {
            return new List<T>(ReadArrayProperty<T>(readElement, isTStruct));
        }

        public List<T> ReadListPropertyComplex<T>(Func<T> readElement, bool isTStruct = false)
        {
            var structName = ReadString();
            if (structName != "RawData")
                throw new InvalidDataException($"Invalid List struct parameter {structName}");
            var typeName = ReadString();
            var size = ReadUInt64();
            var result = ReadListProperty(readElement);

            structName = ReadString();

            if (structName != "None")
                throw new InvalidDataException($"Invalid List struct parameter {structName}");

            return result;
        }




        //public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(
        //        Func<TKey> readKey, Func<TValue> readValue, bool isStruct = false)
        //    where TKey : notnull
        //{
        //    var count = ReadUInt32();
        //    var result = new Dictionary<TKey, TValue>();
        //    if (isStruct)
        //    {
        //        var propName = ReadString();
        //        var propType = ReadString();
        //        ReadUInt64();
        //        var typeName = ReadString();
        //        var guid = ReadGuid();
        //        Skip(1);
        //    }
        //    for (var i = 0; i < count; i++)
        //        result.Add(readKey(), readValue());
        //    return result;
        //}

        //public Dictionary<TKey, TValue> ReadDictionaryProperty<TKey, TValue>(
        //        Func<TKey> readKey, Func<TValue> readValue, bool isStruct = false)
        //    where TKey : notnull
        //{
        //    var arrayType = ReadString();
        //    var optionalGuid = ReadGuidOptional();
        //    return ReadDictionary<TKey, TValue>(readKey, readValue, isStruct);
        //}



        public ByteProperty ReadByteProperty()
        {
            var enumType = ReadString();
            var optionalGuid = ReadGuidOptional();
            if (enumType == "None")
                return new ByteProperty(enumType, optionalGuid, ReadByte(), null);
            else
                return new ByteProperty(enumType, optionalGuid, null, ReadString());
        }




        public ushort ReadUInt16Property()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadUInt16();
        }




        public int ReadInt32Property()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadInt32();
        }

        public int ReadInt32PropertyComplex(string fieldName)
        {
            var structName = ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Unknown SlotID struct parameter {structName}");
            var typeName = ReadString();
            var size = ReadUInt64();

            var result = ReadInt32Property();

            structName = ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Unknown SlotID struct parameter {structName}");

            return result;
        }

        public uint ReadUInt32Property()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadUInt32();
        }

        public void SkipUInt32Property(ulong size)
        {
            ReadUInt32Property();
        }

        public long ReadInt64Property()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadInt64();
        }


        public long ReadInt64PropertyComplex(string fieldName)
        {
            var structName = ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Unknown Int64 struct parameter {structName}");
            var typeName = ReadString();
            var size = ReadUInt64();

            var result = ReadInt64Property();

            structName = ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Unknown Int64 struct parameter {structName}");

            return result;
        }

        public float ReadFloatProperty()
        {
            var optionalGuid = ReadGuidOptional();
            return ReadFloat();
        }

        public bool ReadBoolProperty()
        {
            var result = ReadBoolean();
            var optionalGuid = ReadGuidOptional();
            return result;
        }




        public int ReadInt32Serialized(int bitCount)
        {
            //bitCount - количество бит, которым закодировано число
            var data = ReadBytes((bitCount + 7) / 8);       //Читаем не менее указанного количества бит
            if (bitCount % 8 != 0)     //Если нужное количество бит не равно целому количеству байт
                data[data.Length - 1] &= (byte)((1 << (bitCount % 8)) - 1);     //Обнуляем лишние биты
            Array.Resize(ref data, 4);
            var result = BitConverter.ToInt32(data, 0);

            var signBitMask = 1 << (bitCount - 1);      //Маска для выделения знакового бита
            //Из числа с удалённым знаковым битом вычитаем число, представленное только знаковым битом
            //Если знаковый бит 0, то вычитается 0
            //Если знаковый бит 1, то после вычитания получаются те же значения битов, но уже в виде дополнительно кода
            result = (result & (signBitMask - 1)) - (result & signBitMask);

            return result;
        }

        public Vector3D ReadVector3DPacked(int scaleFactor)
        {
            var componentBitCounAndExtraInfo = ReadUInt32();
            var componentBitCount = componentBitCounAndExtraInfo & 63;
            var extraInfo = componentBitCounAndExtraInfo >> 6;

            if (componentBitCount > 0) {
                var rawX = ReadInt32Serialized((int)componentBitCount);
                var rawY = ReadInt32Serialized((int)componentBitCount);
                var rawZ = ReadInt32Serialized((int)componentBitCount);

                if (extraInfo > 0)
                    return new Vector3D(rawX / (double)scaleFactor, rawY / (double)scaleFactor, rawZ / (double)scaleFactor);

                return new(rawX, rawY, rawZ);
            } else {
                var receivedScalerTypeSize = extraInfo > 0 ? 8 : 4;
                if (receivedScalerTypeSize == 8)
                    return ReadVector3D();
                else
                    return new(ReadFloat(), ReadFloat(), ReadFloat());
            }
        }

        public RotationD ReadShortRotatorCompressed()
        {
            var sPitch = (ushort)(ReadBoolean() ? ReadUInt16() : 0);
            var sYaw = (ushort)(ReadBoolean() ? ReadUInt16() : 0);
            var sRoll = (ushort)(ReadBoolean() ? ReadUInt16() : 0);

            return new(sPitch * doubleCompressCoefficient,
                sYaw * doubleCompressCoefficient,
                sRoll * doubleCompressCoefficient);
        }
    }
}
