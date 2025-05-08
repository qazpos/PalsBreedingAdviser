using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class CharacterContainer
    {
        public bool IsReferenceSlot { get; private set; }
        public CharacterContainerSlot[]? Slots { get; private set; }
        public int SlotNum { get; private set; }
        public byte[]? RawData { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static CharacterContainer Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new CharacterContainer();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "bReferenceSlot":
                        result.IsReferenceSlot = reader.ReadBoolProperty(); break;
                    case "Slots":
                        result.Slots = reader.ReadArrayProperty(() => CharacterContainerSlot.Read(reader, messages), true); break;
                    case "SlotNum":
                        result.SlotNum = reader.ReadInt32Property(); break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown CharacterContainer struct {structName}");
                        localMessages.Add(new Message("StructName", "CharacterContainer", $"Unknown structName {structName} of type {typeName}", null));
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

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("CharacterContainer raw data invalid length");
            }
        }
    }
}
