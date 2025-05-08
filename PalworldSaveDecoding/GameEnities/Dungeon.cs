using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class Dungeon
    {
        public Guid InstanceId { get; private set; }
        public string? DungeonType { get; private set; }
        public Guid MarkerPointId { get; private set; }
        public string? DungeonSpawnAreaId { get; private set; }
        public string? DungeonLevelName { get; private set; }
        public string? BossState { get; private set; }
        public string? EnemySpawnerDataBossRowName { get; private set; }
        public DateTime DisappearTimeAt { get; private set; }
        public int ReservedDataLayerAssetIndex { get; private set; }
        public StageInstanceId? StageInstanceid { get; private set; }
        public MapObject[]? MapObjectSaveData { get; private set; }
        public DateTime RespawnBossTimeAt { get; private set; }




        public static Dungeon Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new Dungeon();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "InstanceId":
                        result.InstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "DungeonType":
                        result.DungeonType = reader.ReadEnumPropertyAsString(); break;
                    case "MarkerPointId":
                        result.MarkerPointId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "DungeonSpawnAreaId":
                        result.DungeonSpawnAreaId = reader.ReadStringProperty(); break;
                    case "DungeonLevelName":
                        result.DungeonLevelName = reader.ReadStringProperty(); break;
                    case "BossState":
                        result.BossState = reader.ReadEnumPropertyAsString(); break;
                    case "EnemySpawnerDataBossRowName":
                        result.EnemySpawnerDataBossRowName = reader.ReadStringProperty(); break;
                    case "DisappearTimeAt":
                        result.DisappearTimeAt = StructProperty.ReadSP(reader, () => reader.ReadDateTimePropertyComplex("Ticks")); break;
                    case "ReservedDataLayerAssetIndex":
                        result.ReservedDataLayerAssetIndex = reader.ReadInt32Property(); break;
                    case "StageInstanceId":
                        result.StageInstanceid = StageInstanceId.ReadComplex(reader, true, messages); break;
                    case "MapObjectSaveData":
                        result.MapObjectSaveData = reader.ReadArrayProperty(() => MapObject.Read(reader, messages), true); break;
                    case "RespawnBossTimeAt":
                        result.RespawnBossTimeAt = StructProperty.ReadSP(reader, () => reader.ReadDateTimePropertyComplex("Ticks")); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Dungeon struct {structName}");
                        localMessages.Add(new Message("StructName", "Dungeon", $"Unknown structName {structName} of type {typeName}", null));
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
