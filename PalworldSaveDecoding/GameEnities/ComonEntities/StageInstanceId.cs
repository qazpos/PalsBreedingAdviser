using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class StageInstanceId : StructProperty
    {
        public Guid InternalId { get; private set; }
        public bool IsValid { get; private set; }




        public static StageInstanceId ReadComplex(GvasFileReader reader, bool isStructProperty = false, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new StageInstanceId();
            if (isStructProperty)
                result.Header = StructPropertyHeader.Read(reader);
            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "InternalId":
                        result.InternalId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "bValid":
                        result.IsValid = reader.ReadBoolProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown StageInstanceId struct parameter {structName}");
                        localMessages.Add(new Message("StructName", "StageInstanceId", $"Unknown structName {structName} of type {typeName}", null));
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


        public static StageInstanceId Read(GvasFileReader reader)
        {
            var result = new StageInstanceId();
            result.InternalId = reader.ReadGuid();
            result.IsValid = reader.ReadInt32() > 0;
            return result;
        }
    }
}
