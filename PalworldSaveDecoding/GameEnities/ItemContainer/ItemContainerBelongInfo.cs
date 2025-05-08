using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class ItemContainerBelongInfo : StructProperty
    {
        public Guid GroupId { get; private set; }
        public bool ControllableOthers { get; private set; }




        public static ItemContainerBelongInfo Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new ItemContainerBelongInfo();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "GroupId" or "GroupID":
                        result.GroupId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "bControllableOthers":
                        result.ControllableOthers = reader.ReadBoolProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown ItemContainerBelongInfo struct {structName}");
                        localMessages.Add(new Message("StructName", "ItemContainerBelongInfo", $"Unknown structName {structName} of type {typeName}", null));
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
