using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class WorkSuitabilityOptionInfo : StructProperty
    {
        public PalWorkSuitability[]? OffWorkSuitabilityList { get; private set; }
        public bool AllowBaseCampBattle { get; private set; }



        public static WorkSuitabilityOptionInfo Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new WorkSuitabilityOptionInfo();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "OffWorkSuitabilityList":
                        result.OffWorkSuitabilityList = reader.ReadArrayProperty(reader.ReadEnum<PalWorkSuitability>); break;
                    case "bAllowBaseCampBattle":
                        result.AllowBaseCampBattle = reader.ReadBoolProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown WorkSuitabilityOptionInfo struct {structName}");
                        localMessages.Add(new Message("StructName", "WorkSuitabilityOptionInfo", $"Unknown structName {structName} of type {typeName}", null));
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
