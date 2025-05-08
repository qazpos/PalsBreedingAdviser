namespace PalworldSaveDecoding
{
    public class MapPropertyHeader
    {
        string? keyType;
        string? valueType;
        Guid? id;


        public static MapPropertyHeader Read(GvasFileReader reader)
        {
            var result = new MapPropertyHeader();
            result.keyType = reader.ReadString();
            result.valueType = reader.ReadString();
            result.id = reader.ReadGuidOptional();
            return result;
        }


        public static void Skip(GvasFileReader reader)
        {
            Read(reader);
        }
    }
}
