using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class SpawnInfo
    {
        public long NextLotteryGameTime { get; private set; }
        public Guid MpaobjectInstanceId { get; private set; }


        

        public static SpawnInfo Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new SpawnInfo();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "NextLotteryGameTime":
                        result.NextLotteryGameTime = reader.ReadInt64Property(); break;
                    case "MapObjectInstanceId":
                        result.MpaobjectInstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown SpawnInfo struct {structName}");
                        localMessages.Add(new Message("StructName", "SpawnInfo", $"Unknown structName {structName} of type {typeName}", null));
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
    }
}
