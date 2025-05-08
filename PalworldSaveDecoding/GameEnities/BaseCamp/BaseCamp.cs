using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class BaseCamp
    {
        public BaseCampWorkerDirector? WorkerDirector { get; private set; }
        public BaseCampWorkCollection? WorkCollection { get; private set; }
        public MapProperty<PalBaseCampModuleType, BaseCampModule>? ModuleMap { get; private set; }

        public byte[]? RawData { get; private set; }
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public byte State { get; private set; }
        public Transform Transform { get; private set; }
        public float AreaRange { get; private set; }
        public Guid GroupIdBelongTo { get; private set; }
        public Transform FastTravelLocalTransform { get; private set; }
        public Guid OwnerMapObjectInstanceId { get; private set; }

        public byte[]? CustomVersionData { get; private set; }



        public static BaseCamp Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new BaseCamp();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "WorkerDirector":
                        result.WorkerDirector = BaseCampWorkerDirector.Read(reader, messages); break;
                    case "WorkCollection":
                        result.WorkCollection = BaseCampWorkCollection.Read(reader, messages); break;
                    case "ModuleMap":
                        result.ModuleMap = MapProperty<PalBaseCampModuleType, BaseCampModule>.Read(
                            reader, reader.ReadEnum<PalBaseCampModuleType>, x => BaseCampModule.Read(reader, x, messages));
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown BaseCamp struct {structName}");
                        localMessages.Add(new Message("StructName", "BaseCamp", $"Unknown structName {structName} of type {typeName}", null));
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
                Id = reader.ReadGuid();
                Name = reader.ReadString();
                State = reader.ReadByte();
                Transform = reader.ReadTransform();
                AreaRange = reader.ReadFloat();
                GroupIdBelongTo = reader.ReadGuid();
                FastTravelLocalTransform = reader.ReadTransform();
                OwnerMapObjectInstanceId = reader.ReadGuid();

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("BaseCamp raw data invalid length");
            }
        }
    }
}
