using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class MapObjectBuildProcess : StructProperty
    {
        public byte[]? RawData { get; private set; }
        public byte State { get; private set; }
        public Guid Id { get; private set; }
        public byte[]? CustomVersionData { get; private set; }




        public static MapObjectBuildProcess Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObjectBuildProcess();
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
                            throw new InvalidDataException($"Unknown MapObjectModelBuildProcess struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectModelBuildProcess", $"Unknown structName {structName} of type {typeName}", null));
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
                State = reader.ReadByte();
                Id = reader.ReadGuid();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("MapObjectModel BuildProcess invalid length");
            }
        }
    }
}
