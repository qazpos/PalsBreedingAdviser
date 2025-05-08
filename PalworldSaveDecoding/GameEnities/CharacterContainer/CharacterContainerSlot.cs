using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class CharacterContainerSlot
    {
        public int SlotIndex { get; private set; }
        public IndividualId? IndividualId { get; private set; }
        public string? PermissionTribeId { get; private set; }

        public byte[]? RawData { get; private set; }
        public IndividualId? RawDataIndividualId { get; private set; }
        public byte RawDataPermissionTribeId { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static CharacterContainerSlot Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new CharacterContainerSlot();
            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "SlotIndex":
                        result.SlotIndex = reader.ReadInt32Property(); break;
                    case "IndividualId":
                        result.IndividualId = StructProperty.ReadSP(reader, () => IndividualId.ReadComplex(reader, messages)); break;
                    case "PermissionTribeID":
                        result.PermissionTribeId = reader.ReadEnumPropertyAsString(); break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown CharacterContainerSlot struct {structName}");
                        localMessages.Add(new Message("StructName", "CharacterContainerSlot", $"Unknown structName {structName} of type {typeName}", null));
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

            using (var reader = new GvasFileReader(new MemoryStream(data), true)) {
                RawDataIndividualId = IndividualId.Read(reader, false);
                RawDataPermissionTribeId = reader.ReadByte();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("CharacterContainer raw data invalid length");
            }
        }
    }
}
