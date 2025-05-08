namespace PalworldSaveDecoding
{
    public class MapObjectConnect
    {
        public byte Index { get; set; }
        public (Guid ConnectToModelInstanceId, byte Index)[]? ConnectsInfo { get; private set; }


        public static MapObjectConnect Read(GvasFileReader reader)
        {
            var result = new MapObjectConnect();
            result.Index = reader.ReadByte();
            result.ConnectsInfo = reader.ReadArray(() => (reader.ReadGuid(), reader.ReadByte()));
            return result;
        }
    }
}
