using System.Runtime.Serialization;

namespace PalworldSaveDecoding
{
    public class MapProperty<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
        public MapPropertyHeader Header { get; set; } = new MapPropertyHeader();
        public uint ReadCount { get; private set; }
        private uint unknownNumber;




        public static MapProperty<TKey, TValue> Read(GvasFileReader reader, Func<TKey> readKey, Func<TValue> readValue) =>
            Read(reader, readKey, x => readValue());


        public static MapProperty<TKey, TValue> Read(GvasFileReader reader, Func<TKey> readKey, Func<TKey, TValue> readValue)
        {
            var result = new MapProperty<TKey, TValue>();
            result.Header = MapPropertyHeader.Read(reader);
            result.unknownNumber = reader.ReadUInt32();
            result.ReadCount = reader.ReadUInt32();

            for (int i = 0; i < result.ReadCount; i++)
            {
                var key = readKey();
                result.Add(key, readValue(key));
            }

            return result;
        }


        public static void Skip(GvasFileReader reader, ulong size)
        {
            MapPropertyHeader.Skip(reader);
            reader.Skip(size);
        }




        public static MapProperty<TKey, TValue> ReadComplex(GvasFileReader reader, Func<TKey> readKey, Func<TKey, TValue> readValue, string fieldName, bool isStructProperty = false)
        {
            var structName = reader.ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Unknown MapProperty struct {structName}");

            var typeName = reader.ReadString();
            var size = reader.ReadUInt64();

            var result = Read(reader, readKey, readValue);

            structName = reader.ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Unknown MapProperty struct {structName}");

            return result;
        }

        public static MapProperty<TKey, TValue> ReadComplex(GvasFileReader reader, Func<TKey> readKey, Func<TValue> readValue, string fieldName)
        {
            return ReadComplex(reader, readKey, x => readValue(), fieldName);
        }
    }
}
