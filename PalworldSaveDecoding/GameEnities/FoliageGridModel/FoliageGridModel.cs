using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class FoliageGridModel
    {
        public MapProperty<Guid, FoliageGridModelInstance> InstanceDataMap { get; private set; } = new();

        public byte[]? RawData { get; private set; }
        public string? ModelId { get; private set; }
        public byte FoliagePresetType { get; private set; }
        public Vector3L CellCoord { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static FoliageGridModel Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new FoliageGridModel();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "InstanceDataMap":
                        result.InstanceDataMap = MapProperty<Guid, FoliageGridModelInstance>.Read(
                            reader, () => StructProperty.ReadSPComplex(reader, reader.ReadGuid, "Guid"), () => FoliageGridModelInstance.Read(reader, messages));
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown FoliageGridModel struct {structName}");
                        localMessages.Add(new Message("StructName", "FoliageGridModel", $"Unknown structName {structName} of type {typeName}", null));
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
                ModelId = reader.ReadString();
                FoliagePresetType = reader.ReadByte();
                CellCoord = reader.ReadVector3L();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("FoliageGridModel raw data invalid length");
            }
        }
    }
}
