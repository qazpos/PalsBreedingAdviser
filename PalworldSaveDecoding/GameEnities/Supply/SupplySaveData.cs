using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class SupplySaveData : StructProperty
    {
        public Guid LastSupplyGuid { get; private set; }
        public DateTime LastSupplyTime { get; private set; }
        public DateTime LastLotteryTime { get; private set; }
        public MapProperty<Guid, SupplyInfo> SupplyInfos { get; private set; } = new();




        public static SupplySaveData Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new SupplySaveData();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "LastSupplyGuid":
                        result.LastSupplyGuid = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "LastSupplyTime":
                        result.LastSupplyTime = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "LastLotteryTime":
                        result.LastLotteryTime = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "SupplyInfos":
                        result.SupplyInfos = MapProperty<Guid, SupplyInfo>.Read(reader, reader.ReadGuid, () => SupplyInfo.Read(reader, messages)); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown SupplySaveData struct {structName}");
                        localMessages.Add(new Message("StructName", "SupplySaveData", $"Unknown structName {structName} of type {typeName}", null));
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
