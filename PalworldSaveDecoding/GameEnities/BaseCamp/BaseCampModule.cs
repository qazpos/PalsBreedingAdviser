using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class BaseCampModule
    {
        public byte[]? RawData { get; private set; }
        public ((ItemId ItemId, uint Num), Vector3D CharacterLocation)[]? TransportItemCharacterInfoReader { get; private set; }
        public (byte Type, byte WorkHardType, byte[] UnknownBytes)[]? PassiveEffects { get; private set; }

        public byte[]? CustomVersionData { get; private set; }

        private static readonly List<PalBaseCampModuleType> noOpTypes = new()
        {
            PalBaseCampModuleType.Energy,
            PalBaseCampModuleType.Medical,
            PalBaseCampModuleType.ResourceCollector,
            PalBaseCampModuleType.ItemStorages,
            PalBaseCampModuleType.FacilityReservation,
            PalBaseCampModuleType.ObjectMaintenance,
        };




        public static BaseCampModule Read(GvasFileReader reader, PalBaseCampModuleType baseCampModuleType, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new BaseCampModule();

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData, baseCampModuleType);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown BaseCampModule struct {structName}");
                        localMessages.Add(new Message("StructName", "BaseCampModule", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecodeRawData(byte[] data, PalBaseCampModuleType baseCampModuleType)
        {
            if (!noOpTypes.Contains(baseCampModuleType))
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true)) {
                switch (baseCampModuleType) {
                    case PalBaseCampModuleType.TransportItemDirector:
                        TransportItemCharacterInfoReader = reader.ReadArray(
                            () => ((ItemId.Read(reader), reader.ReadUInt32()), reader.ReadVector3D()));
                        break;
                    case PalBaseCampModuleType.PassiveEffect:
                        PassiveEffects = reader.ReadArray(() => ReadPassiveEffect(reader)); break;
                    default:
                        break;
                }

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("BaseCampModule raw data invalid length");
            }
        }


        private static (byte Type, byte WorkHardType, byte[] UnknownBytes) ReadPassiveEffect(GvasFileReader reader)
        {
            var type = reader.ReadByte();
            if (type < Enum.GetNames(typeof(PalBaseCampPassiveEffectType)).Length)
                return (type, reader.ReadByte(), reader.ReadBytes(4));
            else
                throw new InvalidDataException($"Unknown BaseCampModule passive effect type {type}");
        }
    }
}
