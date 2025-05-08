using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class MapObjectModel : StructProperty
    {
        public MapProperty<string, byte[]> EffectMap { get; private set; } = new();
        public MapObjectBuildProcess? BuildProcess { get; private set; }
        public MapObjectConnector? Connector { get; private set; }

        public byte[]? RawData { get; private set; }
        public Guid InstanceId { get; private set; }
        public Guid ConcreteModelInstanceId { get; private set; }
        public Guid BaseCampIdBelongTo { get; private set; }
        public Guid GroupIdBelongTo { get; private set; }
        public int CurrentHp { get; private set; }
        public int MaxHp { get; private set; }
        public Transform InitialTransformCache { get; private set; }
        public Guid RepairWorkId { get; private set; }
        public Guid OwnerSpawnerLevelObjectInstanceId { get; private set; }
        public Guid OwnerInstanceId { get; private set; }
        public Guid BuildPlayerUid { get; private set; }
        public byte InteractRestrictType { get; private set; }
        public StageInstanceId? StageInstanceIdBelongTo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public byte[]? UnknownBytes { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static MapObjectModel Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObjectModel();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "BuildProcess":
                        result.BuildProcess = MapObjectBuildProcess.Read(reader, messages);
                        //result.BuildProcessRawData = StructProperty.ReadSP(reader, () => reader.ReadArrayPropertyComplex(reader.ReadByte, "RawData"));
                        //result.DecodeBuildProcess(result.BuildProcessRawData);
                        break;
                    case "Connector":
                        result.Connector = MapObjectConnector.Read(reader, messages);
                        //result.ConnectorRawData = StructProperty.ReadSP(reader, () => reader.ReadArrayPropertyComplex(reader.ReadByte, "RawData"));
                        //result.DecodeConnector(result.ConnectorRawData);
                        break;
                    case "EffectMap":
                        result.EffectMap = MapProperty<string, byte[]>.Read(
                            reader, reader.ReadString, () => reader.ReadArrayPropertyComplex(reader.ReadByte, "RawData"));
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObjectModel struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectModel", $"Unknown structName {structName} of type {typeName}", null));
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
                InstanceId = reader.ReadGuid();
                ConcreteModelInstanceId = reader.ReadGuid();
                BaseCampIdBelongTo = reader.ReadGuid();
                GroupIdBelongTo = reader.ReadGuid();
                CurrentHp = reader.ReadInt32();
                MaxHp = reader.ReadInt32();
                InitialTransformCache = reader.ReadTransform();
                RepairWorkId = reader.ReadGuid();
                OwnerSpawnerLevelObjectInstanceId = reader.ReadGuid();
                OwnerInstanceId = reader.ReadGuid();
                BuildPlayerUid = reader.ReadGuid();
                InteractRestrictType = reader.ReadByte();
                StageInstanceIdBelongTo = StageInstanceId.Read(reader);
                CreatedAt = reader.ReadDateTime();

                if (!reader.IsBaseStreamEnds)
                    UnknownBytes = reader.ReadToEnd();
                    //throw new InvalidDataException("MapObjectModel raw data invalid length");
            }
        }


        //private void DecodeBuildProcess(byte[] data)
        //{
        //    if (data.Length == 0)
        //        return;

        //    using (var reader = new GvasFileReader(new MemoryStream(data), true))
        //    {
        //        BuildProcess = (reader.ReadByte(), reader.ReadGuid());

        //        if (!reader.IsBaseStreamEnds)
        //            throw new InvalidDataException("MapObjectModel BuildProcess invalid length");
        //    }
        //}


        //private void DecodeConnector(byte[] data)
        //{
        //    if (data.Length == 0)
        //        return;

        //    using (var reader = new GvasFileReader(new MemoryStream(data), true))
        //    {
        //        SupportedLevel = reader.ReadInt32();

        //        while (!reader.IsBaseStreamEnds)
        //            Connects.Add(MapObjectConnect.Read(reader));
        //    }
        //}
    }
}
