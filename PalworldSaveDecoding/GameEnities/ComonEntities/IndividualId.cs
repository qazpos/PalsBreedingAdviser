using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class IndividualId : StructProperty
    {
        public Guid PlayerUId { get; set; }
        public Guid InstanceId { get; set; }
        public string? DebugName { get; set; }




        public static IndividualId ReadComplex(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new IndividualId();
            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "PlayerUId":
                        result.PlayerUId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "InstanceId":
                        result.InstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "DebugName":
                        result.DebugName = reader.ReadStringProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown IndividualId struct {structName}");
                        localMessages.Add(new Message("StructName", "IndividualId", $"Unknown structName {structName} of type {typeName}", null));
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


        public static IndividualId Read(GvasFileReader reader, bool readDebugName = true)
        {
            var result = new IndividualId();
            result.PlayerUId = reader.ReadGuid();
            result.InstanceId = reader.ReadGuid();
            if (readDebugName)
                result.DebugName = reader.ReadStringProperty();
            return result;
        }
    }
}
