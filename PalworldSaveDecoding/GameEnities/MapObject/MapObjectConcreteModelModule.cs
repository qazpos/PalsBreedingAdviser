using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class MapObjectConcreteModelModule
    {
        public byte[]? RawData { get; private set; }

        public Guid TargetContainerId { get; private set; }
        public (byte Attribute, int[] Indexes)[]? SlotAttributeIndexes { get; private set; }
        public byte[]? AllSlotAttributes { get; private set; }
        public bool DropItemAtDisposed { get; private set; }
        public byte UsageType { get; private set; }

        public Guid TargetWorkId { get; private set; }

        public byte SwitchState { get; private set; }

        public byte LockState { get; private set; }
        public string? Password { get; private set; }
        public (Guid PlayerUid, int TryFailedCount, bool TrySuccessCache)[]? PlayerInfos { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static MapObjectConcreteModelModule Read(GvasFileReader reader, string moduleType, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObjectConcreteModelModule();

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData, moduleType);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObjectConcreteModelModule struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectConcreteModelModule", $"Unknown structName {structName} of type {typeName}", null));
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


        public void DecodeRawData(byte[] data, string moduleType)
        {
            //var result = new MapObjectConcreteModelModule();

            using (var reader = new GvasFileReader(new MemoryStream(data), true)) {
                if (reader.IsBaseStreamEnds)
                    return;

                switch (moduleType) {
                    case "EPalMapObjectConcreteModelModuleType::ItemContainer":
                        TargetContainerId = reader.ReadGuid();
                        SlotAttributeIndexes = reader.ReadArray(() => (reader.ReadByte(), reader.ReadArray(reader.ReadInt32)));
                        AllSlotAttributes = reader.ReadArray(reader.ReadByte);
                        DropItemAtDisposed = reader.ReadUInt32() > 0;
                        UsageType = reader.ReadByte();
                        break;
                    case "EPalMapObjectConcreteModelModuleType::CharacterContainer":
                        TargetContainerId = reader.ReadGuid(); break;
                    case "EPalMapObjectConcreteModelModuleType::Workee":
                        TargetWorkId = reader.ReadGuid(); break;
                    case "EPalMapObjectConcreteModelModuleType::Energy":
                        break;
                    case "EPalMapObjectConcreteModelModuleType::StatusObserver":
                        break;
                    case "EPalMapObjectConcreteModelModuleType::ItemStack":
                        break;
                    case "EPalMapObjectConcreteModelModuleType::Switch":
                        SwitchState = reader.ReadByte(); break;
                    case "EPalMapObjectConcreteModelModuleType::PlayerRecord":
                        break;
                    case "EPalMapObjectConcreteModelModuleType::BaseCampPassiveEffect":
                        break;
                    case "EPalMapObjectConcreteModelModuleType::PasswordLock":
                        LockState = reader.ReadByte();
                        Password = reader.ReadString();
                        PlayerInfos = reader.ReadArray(() => (reader.ReadGuid(), reader.ReadInt32(), reader.ReadUInt32() > 0));
                        break;
                    default:
                        throw new InvalidDataException($"Unknown MapObjectConcreteModelModule type {moduleType}");
                }

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("Unknown MapObjectConcreteModelModule structure, EOF not reached");

                //return result;
            }
        }
    }
}
