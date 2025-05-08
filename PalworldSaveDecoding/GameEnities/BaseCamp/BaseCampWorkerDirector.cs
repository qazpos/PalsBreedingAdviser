using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class BaseCampWorkerDirector : StructProperty
    {
        public byte[]? RawData { get; private set; }
        public Guid Id { get; private set; }
        public Transform SpawnTransform { get; private set; }
        public byte CurrentOrderType { get; private set; }
        public byte CurrentBattleType { get; private set; }
        public Guid ContainerId { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static BaseCampWorkerDirector Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new BaseCampWorkerDirector();
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
                            throw new InvalidDataException($"Unknown BaseCampWorkerDirector struct {structName}");
                        localMessages.Add(new Message("StructName", "BaseCampWorkerDirector", $"Unknown structName {structName} of type {typeName}", null));
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
                SpawnTransform = reader.ReadTransform();
                CurrentOrderType = reader.ReadByte();
                CurrentBattleType = reader.ReadByte();
                ContainerId = reader.ReadGuid();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("BaseCampWorkerDirector raw data invalid length");
            }
        }
    }
}
