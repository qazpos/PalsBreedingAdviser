namespace PalworldSaveDecoding
{
    public abstract class StructProperty
    {
        public StructPropertyHeader Header { get; set; } = new StructPropertyHeader();




        public static T ReadSP<T>(GvasFileReader reader, Func<T> readT)
        {
            StructPropertyHeader.Read(reader);
            return readT();
        }


        public static void Skip(GvasFileReader reader, ulong size)
        {
            StructPropertyHeader.Skip(reader);
            reader.Skip(size);
        }


        public static T ReadSPComplex<T>(GvasFileReader reader, Func<T> readT, string fieldName)
        {
            var structName = reader.ReadString();
            if (structName != fieldName)
                throw new InvalidDataException($"Struct parameter {structName} doesn't match the requested {fieldName}");
            var typeName = reader.ReadString();
            var size = reader.ReadUInt64();

            var result = ReadSP(reader, readT);

            structName = reader.ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Unknown Struct property parameter {structName}");

            return result;
        }


        public static T ReadSPComplexSP<T>(GvasFileReader reader, Func<T> readT, string fieldName)
        {
            StructPropertyHeader.Read(reader);
            return ReadSPComplex(reader, readT, fieldName);
        }
    }
}
