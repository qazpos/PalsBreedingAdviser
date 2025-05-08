using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class ItemId : StructProperty
    {
        public string? StaticId;
        public DynamicId? DynamicId;

        public static ItemId Read(GvasFileReader reader)
        {
            var result = new ItemId();
            result.StaticId = reader.ReadString();
            result.DynamicId = DynamicId.Read(reader);
            return result;
        }


        public static ItemId ReadReverse(GvasFileReader reader)
        {
            var result = new ItemId();
            result.DynamicId = DynamicId.Read(reader);
            result.StaticId = reader.ReadString();
            return result;
        }


        public static ItemId ReadComplex(GvasFileReader reader, bool isStructPtoperty = false, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new ItemId();
            if (isStructPtoperty)
                result.Header = StructPropertyHeader.Read(reader);
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "StaticId":
                        result.StaticId = reader.ReadStringProperty(); break;
                    case "DynamicId":
                        result.DynamicId = DynamicId.ReadComplex(reader); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown ItemId struct {structName}");
                        localMessages.Add(new Message("StructName", "ItemId", $"Unknown structName {structName} of type {typeName}", null));
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
