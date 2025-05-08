using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class MapObjectConnector : StructProperty
    {
        public byte[]? RawData { get; private set; }
        public int SupportedLevel { get; private set; }
        public List<MapObjectConnect> Connects { get; private set; } = new();
        public byte[]? CustomVersionData { get; private set; }




        public static MapObjectConnector Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObjectConnector();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObjectConnector struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectConnector", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;

                }

                structName = reader.ReadString();
            }

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = result.ToString();
                }
                messages.AddRange(localMessages);
            }
            return result;
        }


        private void DecodeRawData(byte[] data)
        {
            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true)) {
                SupportedLevel = reader.ReadInt32();

                while (!reader.IsBaseStreamEnds)
                    Connects.Add(MapObjectConnect.Read(reader));
            }
        }
    }
}
