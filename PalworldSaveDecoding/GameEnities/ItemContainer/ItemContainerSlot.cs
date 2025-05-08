using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class ItemContainerSlot
    {
        public int SlotId { get; private set; }
        public ItemId? ItemId { get; private set; }
        public int StackCount { get; private set; }

        public byte[]? RawData { get; private set; }
        public (byte[] TypeA, byte[] TypeB, string[] ItemStaticIds) Permission { get; private set; }
        public float CorruptionProgressValue { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static ItemContainerSlot Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new ItemContainerSlot();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "SlotIndex":
                        result.SlotId = reader.ReadInt32Property(); break;
                    case "ItemId":
                        result.ItemId = ItemId.ReadComplex(reader, true, messages); break;
                    case "StackCount":
                        result.StackCount = reader.ReadInt32Property(); break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        //result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown ItemContainerSlot struct {structName}");
                        localMessages.Add(new Message("StructName", "ItemContainerSlot", $"Unknown structName {structName} of type {typeName}", null));
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
                Permission = (reader.ReadArray(reader.ReadByte),
                    reader.ReadArray(reader.ReadByte),
                    reader.ReadArray(reader.ReadString));
                CorruptionProgressValue = reader.ReadFloat();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("ItemContainerSlot raw data invalid length");
            }
        }
    }
}
