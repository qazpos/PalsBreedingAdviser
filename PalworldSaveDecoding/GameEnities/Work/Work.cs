using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class Work
    {
        public string? WorkableType { get; private set; }
        public MapProperty<int, WorkAssign> WorkAssignMap { get; private set; } = new();

        public byte[]? RawData { get; private set; }
        public Guid Id { get; private set; }
        public WorkableBounds? WorkableBounds { get; private set; }
        public Guid BaseCampIdBelongTo { get; private set; }
        public Guid OwnerMapObjectModelId { get; private set; }
        public Guid OwnerMapObjectConcreteModelId { get; private set; }
        public byte CurrentState { get; private set; }
        public (Vector3D Location, Vector3D FasingDirecton)[]? AssignLocations { get; private set; }
        public byte BehaviourType { get; private set; }
        public string? AssignDefineDataId { get; private set; }
        public byte OverrideWorkType { get; private set; }
        public byte AssignableFixedType { get; private set; }
        public bool AssignableOtomo { get; private set; }
        public bool CanTriggerWorkerEvent { get; private set; }
        public bool CanStealAssign { get; private set; }
        public float RequiredWorkAmount { get; private set; }
        public int WorkExp { get; private set; }
        public float CurrentWorkAmount { get; private set; }
        public float AutoWorkSelfAmountBySec { get; private set; }
        public WorkTransform TransformData { get; private set; } = new();

        public byte DefenseCombatType { get; private set; }
        public IndividualId? TargetIndividualId { get; private set; }

        public Guid HandleId { get; private set; }
        public int LocationIndex { get; private set; }
        public byte AssignType { get; private set; }
        public IndividualId? AssignedIndividualId { get; private set; }
        public byte State { get; private set; }
        public uint Fixed { get; private set; }
        public Guid TargetMapObjectModelId { get; private set; }

        public byte[]? CustomVersionData { get; private set; }


        private static readonly List<string> WorkBaseTypes = new()
        {
            //"EPalWorkableType::Illegal",
            "EPalWorkableType::Progress",
            //"EPalWorkableType::CollectItem",
            //"EPalWorkableType::TransportItem",
            "EPalWorkableType::TransportItemInBaseCamp",
            "EPalWorkableType::ReviveCharacter",
            //"EPalWorkableType::CollectResource",
            "EPalWorkableType::LevelObject",
            "EPalWorkableType::Repair",
            "EPalWorkableType::Defense",
            "EPalWorkableType::BootUp",
            "EPalWorkableType::OnlyJoin",
            "EPalWorkableType::OnlyJoinAndWalkAround",
            "EPalWorkableType::RemoveMapObjectEffect",
            "EPalWorkableType::MonsterFarm",
        };




        public static Work Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new Work();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "WorkableType":
                        result.WorkableType = reader.ReadEnumPropertyAsString(); break;
                    case "WorkAssignMap":
                        result.WorkAssignMap = MapProperty<int, WorkAssign>.Read(
                            reader, reader.ReadInt32, () => WorkAssign.Read(reader, messages));
                        break;
                    case "RawData":
                        if (result.WorkableType == null)
                            throw new InvalidDataException("Unknown Work structure. Reading RawData without WorkableType");
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData, result.WorkableType);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Work struct {structName}");
                        localMessages.Add(new Message("StructName", "Work", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecodeRawData(byte[] data, string workType)
        {
            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                if (WorkBaseTypes.Contains(workType))
                {
                    Id = reader.ReadGuid();
                    WorkableBounds = WorkableBounds.Read(reader);
                    BaseCampIdBelongTo = reader.ReadGuid();
                    OwnerMapObjectModelId = reader.ReadGuid();
                    OwnerMapObjectConcreteModelId = reader.ReadGuid();
                    CurrentState = reader.ReadByte();
                    AssignLocations = reader.ReadArray(() => (reader.ReadVector3D(), reader.ReadVector3D()));
                    BehaviourType = reader.ReadByte();
                    AssignDefineDataId = reader.ReadString();
                    OverrideWorkType = reader.ReadByte();
                    AssignableOtomo = reader.ReadUInt32() > 0;
                    CanTriggerWorkerEvent = reader.ReadUInt32() > 0;
                    CanStealAssign = reader.ReadUInt32() > 0;

                    switch (workType)
                    {
                        case "PalWorkableType::Defense":
                            DefenseCombatType = reader.ReadByte(); break;
                        case "EPalWorkableType::Progress":
                            RequiredWorkAmount = reader.ReadFloat();
                            WorkExp = reader.ReadInt32();
                            CurrentWorkAmount = reader.ReadFloat();
                            AutoWorkSelfAmountBySec = reader.ReadFloat();
                            break;
                        case "EPalWorkableType::ReviveCharacter":
                            TargetIndividualId = IndividualId.Read(reader, false); break;
                    }
                }
                else if (workType == "EPalWorkableType::Assign" || workType == "EPalWorkableType::LevelObject")
                {
                    HandleId = reader.ReadGuid();
                    LocationIndex = reader.ReadInt32();
                    AssignType = reader.ReadByte();
                    AssignedIndividualId = IndividualId.Read(reader, false);
                    State = reader.ReadByte();
                    Fixed = reader.ReadUInt32();

                    if (workType == "EPalWorkableType::LevelObject")
                        TargetMapObjectModelId = reader.ReadGuid();
                }

                //UPalWorkProgressTransformBase->SerializeProperties
                TransformData = WorkTransform.Read(reader);
            }
        }
    }
}
