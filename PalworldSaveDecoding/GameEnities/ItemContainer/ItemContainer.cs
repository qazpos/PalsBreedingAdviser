using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class ItemContainer
    {
        //public Guid BelongInfo { get; private set; }
        public ItemContainerBelongInfo? BelongInfo { get; private set; }
        public ItemContainerSlot[]? Slots { get; private set; }

        public byte[]? RawData { get; private set; }
        public (byte[] TypeA, byte[] TypeB, string[] ItemStaticIds) Permission { get; private set; }
        public byte[]? UnknownBytes { get; private set; }
        public int SlotNum { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static ItemContainer Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new ItemContainer();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "BelongInfo":
                        //result.BelongInfo = StructProperty.ReadSPComplexSP(reader, reader.ReadGuid, "GroupID"); break;
                        result.BelongInfo = ItemContainerBelongInfo.Read(reader, messages); break;
                    case "Slots":
                        result.Slots = reader.ReadArrayProperty(() => ItemContainerSlot.Read(reader, messages), true);
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecoreRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    case "SlotNum":
                        result.SlotNum = reader.ReadInt32Property(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown ItemContainer struct {structName}");
                        localMessages.Add(new Message("StructName", "ItemContainer", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecoreRawData(byte[] data)
        {
            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                Permission = (reader.ReadArray(reader.ReadByte),
                    reader.ReadArray(reader.ReadByte),
                    reader.ReadArray(reader.ReadString));

                if (!reader.IsBaseStreamEnds)
                    UnknownBytes = reader.ReadToEnd();
            }
        }
    }
}
