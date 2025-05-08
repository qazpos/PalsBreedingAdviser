using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class FoodRegeneEffectInfo : StructProperty
    {
        public int? EffectTime { get; private set; }
        public int? RemainingTime { get; private set; }



        public static FoodRegeneEffectInfo Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new FoodRegeneEffectInfo();
            result.Header = StructPropertyHeader.Read(reader);
            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "EffectTime":
                        result.EffectTime = reader.ReadInt32Property(); break;
                    case "RemainingTime":
                        result.RemainingTime = reader.ReadInt32Property(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown FoodRegeneEffectInfo struct {structName}");
                        localMessages.Add(new Message("StructName", "FoodRegeneEffectInfo", $"Unknown structName {structName} of type {typeName}", null));
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
