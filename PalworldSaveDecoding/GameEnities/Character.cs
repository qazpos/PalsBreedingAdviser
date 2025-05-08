using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class Character
    {
        public byte[]? RawData;
        public byte[]? CustomVersionData;
        //Player properties
        public bool IsPlayer { get; private set; }
        public int Support { get; private set; }
        public long ShieldMaxHp { get; private set; }
        public long ShieldHp { get; private set; }
        public long MaxSp { get; private set; }
        public ushort UnusedStatusPoint { get; private set; }
        public ByteProperty VoiceId { get; private set; }

        //Pal properties
        public Guid GroupId { get; private set; }
        public string? CharacterId { get; private set; }
        public bool IsFavoritePal { get; private set; }
        public ByteProperty FavoriteIndex { get; private set; }
        public string? UniqueNpcId { get; private set; }
        public PalGenderType? Gender { get; private set; }
        //public List<PalWazaId> EquipWaza { get; private set; } = new List<PalWazaId>();
        public string[]? EquipWaza { get; private set; }
        //public List<PalWazaId> MasteredWaza { get; private set; } = new List<PalWazaId>();
        public string[]? MasteredWaza { get; private set; }
        public long Mp { get; private set; }
        public float MaxFullStomach { get; private set; }
        public ByteProperty TalentHp { get; private set; }
        public int TalentMelee { get; private set; }
        public ByteProperty TalentShot { get; private set; }
        public ByteProperty TalentDefense { get; private set; }
        public string[]? PassiveSkillList { get; private set; }
        public DateTime OwnedTime { get; private set; }
        public Guid OwnerPlayerUid { get; private set; }
        public Guid[]? OldOwnerPlayerUids { get; private set; }
        public Guid ItemContainerId { get; private set; }
        public Guid EquipItemContainerId { get; private set; }
        public (Guid ContainerId, int SlotIndex) SlotId { get; private set; }
        public float SanityValue { get; private set; }
        public float? AffectSanityRates { get; private set; }
        public string? FoodWithStatusEffect { get; private set; }
        public int Timer_FoodWithStatusEffect { get; private set; }
        public ByteProperty Rank { get; private set; }
        public ushort RankUpExp { get; private set; }
        public int RankHp { get; private set; }
        public int RankAttack { get; private set; }
        public int RankDefence { get; private set; }
        public int RankCraftSpeed { get; private set; }
        public bool IsRarePal { get; private set; }
        public PalWorkSuitability CurrentWorkSuitability { get; private set; }
        public WorkSuitabilityOptionInfo? WorkSuitabilityOptionInfo { get; private set; }
        public string? WorkerSick { get; private set; }
        public string? HungerType { get; private set; }
        public string? BaseCampWorkerEventType { get; private set; }
        public float? BaseCampWorkerEventProgressTime { get; private set; }
        public string? PhysicalHealth { get; private set; }
        public FoodRegeneEffectInfo? FoodRegeneEffectInfo { get; private set; }


        //General properties
        public string? NickName { get; private set; }
        public string? FilteredNickName { get; private set; }
        public Guid? LastNickNameModifierPlayerUid { get; private set; }
        public ByteProperty Level { get; private set; }
        public long Exp { get; private set; }
        public long MaxHp { get; private set; }
        public long Hp { get; private set; }
        public float FullStomach { get; private set; }
        public int CraftSpeed { get; private set; }
        public Dictionary<PalWorkSuitability, int> CraftSpeeds { get; private set; } = new Dictionary<PalWorkSuitability, int>();
        public Vector3D LastJumpedLocation { get; private set; }
        public Dictionary<string, int> GotStatusPointList { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> GotExStatusPointList { get; private set; } = new Dictionary<string, int>();
        public float? DecreaseFullStomachRates { get; private set; }
        public float? CraftSpeedRates { get; private set; }




        public static Character Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var result = new Character();
            var localMessages = new MessageCollection();
            var structName = reader.ReadString();

            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData, messages);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte);
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown CharacterValue struct {structName}");
                        localMessages.Add(new Message("StructName", "CharacterValue", $"Unknown structName {structName} of type {typeName}", null));
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
            //result.RawData = reader.ReadArrayPropertyComplex(reader.ReadByte, "RawData");
            //result.DecodeRawData(result.RawData, messages);
            return result;
        }


        public static Character ReadClear(GvasFileReader reader, MessageCollection? messages = null)
        {
            var result = new Character();
            result.ReadRawData(reader, messages);
            return result;
        }


        private void DecodeRawData(byte[] rawData, MessageCollection? messages = null)
        {
            using (var reader = new GvasFileReader(new MemoryStream(rawData), true)) {
                ReadRawData(reader, messages);
            }
        }


        private void ReadRawData(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var _structName = reader.ReadString();
            while (_structName != "None") {
                var _typeName = reader.ReadString();
                var _size = reader.ReadUInt64();

                switch (_structName) {
                    case "SaveParameter":
                        StructPropertyHeader.Read(reader);
                        var structName = reader.ReadString();
                        while (structName != "None") {
                            var typeName = reader.ReadString();
                            var size = reader.ReadUInt64();

                            switch (structName) {
                                case "CharacterID":
                                    CharacterId = reader.ReadStringProperty(); break;
                                case "IsFavoritePal":
                                    IsFavoritePal = reader.ReadBoolProperty(); break;
                                case "FavoriteIndex":
                                    FavoriteIndex = reader.ReadByteProperty(); break;
                                case "UniqueNPCID":
                                    UniqueNpcId = reader.ReadStringProperty(); break;
                                case "Gender":
                                    Gender = reader.ReadEnumProperty<PalGenderType>(); break;
                                case "Level":
                                    Level = reader.ReadByteProperty(); break;
                                case "Exp":
                                    Exp = reader.ReadInt64Property(); break;
                                case "EquipWaza":
                                    EquipWaza = reader.ReadArrayProperty(reader.ReadEnumAsString); break;
                                case "MasteredWaza":
                                    MasteredWaza = reader.ReadArrayProperty(reader.ReadEnumAsString); break;
                                case "HP" or "Hp":
                                    Hp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "Talent_HP":
                                    TalentHp = reader.ReadByteProperty(); break;
                                case "Talent_Melee":
                                    TalentMelee = reader.ReadInt32Property(); break;
                                case "Talent_Shot":
                                    TalentShot = reader.ReadByteProperty(); break;
                                case "Talent_Defense":
                                    TalentDefense = reader.ReadByteProperty(); break;
                                case "FullStomach":
                                    FullStomach = reader.ReadFloatProperty(); break;
                                case "PassiveSkillList":
                                    PassiveSkillList = reader.ReadArrayProperty(reader.ReadString); break;
                                case "MP" or "Mp":
                                    Mp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "OwnedTime":
                                    OwnedTime = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                                case "OwnerPlayerUId":
                                    OwnerPlayerUid = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                                case "OldOwnerPlayerUIds":
                                    OldOwnerPlayerUids = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                                case "MaxHP":
                                    MaxHp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "CraftSpeed":
                                    CraftSpeed = reader.ReadInt32Property(); break;
                                case "CraftSpeeds":
                                    CraftSpeeds = new(reader.ReadArrayProperty(() => ReadCraftSpeedsPair(reader, messages), true)); break;
                                case "ItemContainerId":
                                    ItemContainerId = StructProperty.ReadSPComplexSP(reader, reader.ReadGuid, "ID"); break;
                                case "EquipItemContainerId":
                                    EquipItemContainerId = StructProperty.ReadSPComplexSP(reader, reader.ReadGuid, "ID"); break;
                                case "SlotID" or "SlotId":
                                    StructPropertyHeader.Read(reader);
                                    var subStructName = reader.ReadString();
                                    Guid? containerId = null;
                                    int? slotIndex = null;

                                    while (subStructName != "None") {
                                        var subTypeName = reader.ReadString();
                                        var subSize = reader.ReadUInt64();

                                        switch (subStructName) {
                                            case "ContainerId":
                                                containerId = StructProperty.ReadSPComplexSP(reader, reader.ReadGuid, "ID"); break;
                                            case "SlotIndex":
                                                slotIndex = reader.ReadInt32Property(); break;
                                            default:
                                                if (messages == null)
                                                    throw new InvalidDataException($"Unknown Character.SaveParameter.SlotID struct parameter {subStructName}");
                                                localMessages.Add(new Message("StructName", "Character.SaveParameter.SlotID", $"Unknown structName {subStructName} of type {subTypeName}", null));
                                                reader.Skip(size);
                                                break;
                                        }

                                        subStructName = reader.ReadString();
                                    }
                                    if (containerId != null && slotIndex != null)
                                        SlotId = ((Guid)containerId, (int)slotIndex);
                                    else {
                                        if (messages == null)
                                            throw new InvalidDataException("Character.SaveParameter.SlotID ContainerId or SlotIndex is null");
                                        localMessages.Add(new Message(null, "Character.SaveParameter.SlotID", $"ContainerId or SlotIndex is null", null));
                                    }
                                    break;
                                case "MaxFullStomach":
                                    MaxFullStomach = reader.ReadFloatProperty(); break;
                                case "GotStatusPointList":
                                    GotStatusPointList = new(reader.ReadArrayProperty(() => ReadGotStatusPointPair(reader, messages), true)); break;
                                case "GotExStatusPointList":
                                    GotExStatusPointList = new(reader.ReadArrayProperty(() => ReadGotStatusPointPair(reader, messages), true)); break;
                                case "LastJumpedLocation":
                                    LastJumpedLocation = StructProperty.ReadSP(reader, reader.ReadVector3D); break;
                                case "SanityValue":
                                    SanityValue = reader.ReadFloatProperty(); break;
                                case "DecreaseFullStomachRates":
                                    DecreaseFullStomachRates = ReadFloatContainer(reader); break;
                                case "AffectSanityRates":
                                    AffectSanityRates = ReadFloatContainer(reader); break;
                                case "CraftSpeedRates":
                                    CraftSpeedRates = ReadFloatContainer(reader); break;
                                case "FoodWithStatusEffect":
                                    FoodWithStatusEffect = reader.ReadStringProperty(); break;
                                case "Tiemr_FoodWithStatusEffect":
                                    Timer_FoodWithStatusEffect = reader.ReadInt32Property(); break;
                                case "WorkerSick":
                                    WorkerSick = reader.ReadEnumPropertyAsString(); break;
                                case "HungerType":
                                    HungerType = reader.ReadEnumPropertyAsString(); break;
                                case "BaseCampWorkerEventType":
                                    BaseCampWorkerEventType = reader.ReadEnumPropertyAsString(); break;
                                case "BaseCampWorkerEventProgressTime":
                                    BaseCampWorkerEventProgressTime = reader.ReadFloatProperty(); break;
                                case "PhysicalHealth":
                                    PhysicalHealth = reader.ReadEnumPropertyAsString(); break;
                                case "WorkSuitabilityOptionInfo":
                                    WorkSuitabilityOptionInfo = WorkSuitabilityOptionInfo.Read(reader, messages); break;
                                case "FoodRegeneEffectInfo":
                                    FoodRegeneEffectInfo = FoodRegeneEffectInfo.Read(reader, messages); break;



                                case "NickName":
                                    NickName = reader.ReadStringProperty(); break;
                                case "FilteredNickName":
                                    FilteredNickName = reader.ReadStringProperty(); break;
                                case "LastNickNameModifierPlayerUid":
                                    LastNickNameModifierPlayerUid = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                                case "IsPlayer":
                                    IsPlayer = reader.ReadBoolProperty(); break;
                                case "Support":
                                    Support = reader.ReadInt32Property(); break;
                                case "ShieldHP":
                                    ShieldHp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "ShieldMaxHP":
                                    ShieldMaxHp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "MaxSP":
                                    MaxSp = StructProperty.ReadSP(reader, () => reader.ReadInt64PropertyComplex("Value")); break;
                                case "UnusedStatusPoint":
                                    UnusedStatusPoint = reader.ReadUInt16Property(); break;
                                case "VoiceID":
                                    VoiceId = reader.ReadByteProperty(); break;
                                case "Rank":
                                    Rank = reader.ReadByteProperty(); break;
                                case "RankUpExp":
                                    RankUpExp = reader.ReadUInt16Property(); break;
                                case "IsRarePal":
                                    IsRarePal = reader.ReadBoolProperty(); break;
                                case "Rank_HP":
                                    RankHp = reader.ReadInt32Property(); break;
                                case "Rank_Attack":
                                    RankAttack = reader.ReadInt32Property(); break;
                                case "Rank_Defence":
                                    RankDefence = reader.ReadInt32Property(); break;
                                case "Rank_CraftSpeed":
                                    RankCraftSpeed = reader.ReadInt32Property(); break;
                                case "CurrentWorkSuitability":
                                    CurrentWorkSuitability = reader.ReadEnumProperty<PalWorkSuitability>(); break;
                                default:
                                    if (messages == null)
                                        throw new InvalidDataException($"Unknown Character.SaveParameter struct {structName}");
                                    localMessages.Add(new Message("StructName", "Character.SaveParameter", $"Unknown structName {structName} of type {typeName}", null));
                                    reader.Skip(size);
                                    break;
                            }

                            structName = reader.ReadString();
                        }
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Character struct parameter {_structName}");
                        localMessages.Add(new Message("StructName", "Character", $"Unknown structName {_structName} of type {_typeName}", null));
                        reader.Skip(_size);
                        break;
                }

                _structName = reader.ReadString();
            }

            var unknownBytes = reader.ReadBytes(4);
            GroupId = reader.ReadGuid();

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = ToString();
                }
                messages.AddRange(localMessages);
            }
        }


        private static KeyValuePair<PalWorkSuitability, int> ReadCraftSpeedsPair(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            PalWorkSuitability? key = null;
            int value = 0;

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "WorkSuitability":
                        key = reader.ReadEnumProperty<PalWorkSuitability>(); break;
                    case "Rank":
                        value = reader.ReadInt32Property(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Craft Speeds substruct type {structName}");
                        localMessages.Add(new Message("StructName", "Character.SaveParameter.CraftSpeeds", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }

            if (key == null)
                throw new InvalidDataException($"Invalid Craft Speeds member structure");

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = $"WorkSuitability: {key}, Rank: {value}";
                }
                messages.AddRange(localMessages);
            }
            return new KeyValuePair<PalWorkSuitability, int>((PalWorkSuitability)key, value);
        }


        private static KeyValuePair<string, int> ReadGotStatusPointPair(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            string? key = null;
            int value = 0;

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();
                switch (structName) {
                    case "StatusName":
                        key = reader.ReadStringProperty(); break;
                    case "StatusPoint":
                        value = reader.ReadInt32Property(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Got Status Point substruct type {structName}");
                        localMessages.Add(new Message("StructName", "Character.SaveParameter.GotStatusPointList", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }

            if (key == null)
                throw new InvalidDataException($"Invalid Got Status Point member structure");

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = $"StatusName: {key}, StatusPoint: {value}";
                }
                messages.AddRange(localMessages);
            }
            return new KeyValuePair<string, int>(key, value);
        }


        private static float? ReadFloatContainer(GvasFileReader reader)
        {
            float? result = null;
            StructPropertyHeader.Read(reader);
            var structName = reader.ReadString();
            if (structName != "None")
                throw new InvalidDataException($"Unknown FloatContainer struct parameter {structName}");
            return result;
        }
    }
}