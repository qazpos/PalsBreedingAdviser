using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class DynamicId : StructProperty
    {
        public Guid CreatedWorldId { get; private set; }
        public Guid LocalIdInCreatedWorld { get; private set; }




        public static DynamicId Read(GvasFileReader reader)
        {
            var result = new DynamicId();
            result.CreatedWorldId = reader.ReadGuid();
            result.LocalIdInCreatedWorld = reader.ReadGuid();
            return result;
        }


        public static DynamicId ReadComplex(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new DynamicId();
            result.Header = StructPropertyHeader.Read(reader);
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "CreatedWorldId":
                        result.CreatedWorldId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "LocalIdInCreatedWorld":
                        result.LocalIdInCreatedWorld = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown DynamicId struct {structName}");
                        localMessages.Add(new Message("StructName", "DynamicId", $"Unknown structName {structName} of type {typeName}", null));
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
