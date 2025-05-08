using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class BaseCampWorkCollection : StructProperty
    {
        public byte[]? RawData { get; private set; }
        public Guid Id { get; private set; }
        public Guid[]? WorkIds { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static BaseCampWorkCollection Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new BaseCampWorkCollection();
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
                            throw new InvalidDataException($"Unknown BaseCamp struct {structName}");
                        localMessages.Add(new Message("StructName", "BaseCamp", $"Unknown structName {structName} of type {typeName}", null));
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
                Id = reader.ReadGuid();
                WorkIds = reader.ReadArray(reader.ReadGuid);

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("BaseCampWorkCollection raw data invalid length");
            }
        }
    }
}
