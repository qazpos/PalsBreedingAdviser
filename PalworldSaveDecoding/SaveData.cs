using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class SaveData : StructProperty
    {
        public string? WorldName { get; private set; }
        public string? HostPlayerName { get; private set; }
        public int HostPlayerLevel { get; private set; }
        public int InGameDay { get; private set; }




        public static SaveData Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new SaveData();

            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "WorldName":
                        result.WorldName = reader.ReadStringProperty(); break;
                    case "HostPlayerName":
                        result.HostPlayerName = reader.ReadStringProperty(); break;
                    case "HostPlayerLevel":
                        result.HostPlayerLevel = reader.ReadInt32Property(); break;
                    case "InGameDay":
                        result.InGameDay = reader.ReadInt32Property(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown LevelMeta.SaveData struct {structName}");
                        localMessages.Add(new Message("StructName", "LevelMeta.SaveData", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }

            if (messages != null)
                messages.AddRange(localMessages);
            return result;
        }
    }
}
