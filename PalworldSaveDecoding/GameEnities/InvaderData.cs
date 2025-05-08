using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class InvaderData
    {
        public bool IsInvading { get; private set; }
        public float CoolTimeElapsed { get; private set; }
        public float CoolTimeFinish { get; private set; }



        public static InvaderData Read(GvasFileReader reader, MessageCollection? messages)
        {
            var localMessages = new MessageCollection();
            var result = new InvaderData();

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "bIsInvading":
                        result.IsInvading = reader.ReadBoolProperty(); break;
                    case "CoolTimeElapsed":
                        result.CoolTimeElapsed = reader.ReadFloatProperty(); break;
                    case "CoolTimeFinish":
                        result.CoolTimeFinish = reader.ReadFloatProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown InvaderData struct {structName}");
                        localMessages.Add(new Message("StructName", "InvaderData", $"Unknown structName {structName} of type {typeName}", null));
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
