namespace PalworldSaveDecoding
{
    public class StructPropertyHeader
    {
        string? structType;
        Guid? structId;
        Guid? id;

        public static StructPropertyHeader Read(GvasFileReader gvasFileReader)
        {
            var result = new StructPropertyHeader();
            result.structType = gvasFileReader.ReadString();
            result.structId = gvasFileReader.ReadGuid();
            result.id = gvasFileReader.ReadGuidOptional();
            return result;
        }

        public static void Skip(GvasFileReader gvasFileReader)
        {
            Read(gvasFileReader);
        }
    }
}
