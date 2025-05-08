using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class WorldSaveData : StructProperty
    {
        public MapProperty<IndividualId, Character> CharacterSaveParameterMap { get; private set; } = new();
        public MapObject[]? MapObjectSaveData { get; private set; }
        public MapProperty<Vector3L, MapProperty<string, FoliageGridModel>> FoliageGridSaveDataMap { get; private set; } = new();
        public MapProperty<StageInstanceId, MapProperty<Guid, MapProperty<int, SpawnInfo>>> MapObjectSpawnerInStageSaveData { get; private set; } = new();
        public Work[]? WorkSaveData { get; private set; }
        public MapProperty<Guid, BaseCamp> BaseCampSaveData { get; private set; } = new();
        public MapProperty<Guid, ItemContainer> ItemContainerSaveData { get; private set; } = new();
        public DynamicItem[]? DynamicItemSaveData { get; private set; }
        public MapProperty<Guid, CharacterContainer> CharacterContainerSaveData { get; private set; } = new();
        public MapProperty<Guid, Group> GroupSaveDataMap { get; private set; } = new();
        public (long GameDateTimeTicks, long RealDateTimeTicks) GameTimeSaveData { get; private set; }
        public MapProperty<string, EnemyCampStatus> EnemyCampSaveData { get; private set; } = new();
        public DungeonPointMarker[]? DungeonPointMarkerSaveData { get; private set; }
        public Dungeon[]? DungeonSaveData { get; private set; }
        public MapProperty<string, bool> BossSpawnerSaveData { get; private set; } = new();
        public MapProperty<Guid, InvaderData> InvaderSaveData { get; private set; } = new();
        public MapProperty<PalOilrigType, OilrigData> OilrigMap { get; private set; } = new();
        public SupplySaveData? SupplyData { get; private set; }
        public uint WorldMetaSaveVersionBitMask { get; private set; }


        static ProgressType progressReportType = ProgressType.Level;

        static readonly Dictionary<string, string> subPaths = new Dictionary<string, string>() {
            { "CharacterSaveParameterMap", "CharacterSaveParameterMap" },
            { "MapObjectSaveData", "MapObjectSaveData" },
            { "FoliageGridSaveDataMap", "FoliageGridSaveDataMap" },
            { "MapObjectSpawnerInStageSaveData", "MapObjectSpawnerInStageSaveData" },
            { "WorkSaveData", "WorkSaveData" },
            { "BaseCampSaveData", "BaseCampSaveData" },
            { "ItemContainerSaveData", "ItemContainerSaveData" },
            { "DynamicItemSaveData", "DynamicItemSaveData" },
            { "CharacterContainerSaveData", "CharacterContainerSaveData" },
            { "GroupSaveDataMap", "GroupSaveDataMap" },
            { "GameTimeSaveData", "GameTimeSaveData" },
            { "EnemyCampSaveData", "EnemyCampSaveData" },
            { "DungeonPointMarkerSaveData", "DungeonPointMarkerSaveData" },
            { "DungeonSaveData", "DungeonSaveData" },
            { "BossSpawnerSaveData", "BossSpawnerSaveData" },
            { "InvaderSaveData", "InvaderSaveData" },
            { "OilrigSaveData", "OilrigMap" },
            { "SupplySaveData", "SupplyData" },
            { "WorldMetaSaveVersionBitMask", "WorldMetaSaveVersionBitMask" },
        };




        public static WorldSaveData Read(GvasFileReader reader, ulong sizeFromParent, IProgress<SaveReadingProgressData>? progress,
            MessageCollection? messages = null, SavePathsList? pathsList = null, string pathFromParent = "")
        {
            var localMessages = new MessageCollection();
            var result = new WorldSaveData();

            var isFilteredReading = pathsList != null && pathsList.Count > 0;

            result.Header = StructPropertyHeader.Read(reader);
            var readerBytesShouldLeft = reader.BytesLeft - sizeFromParent;

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();
                var bytesLeft = reader.BytesLeft;

                var subPathFull = "";
                var needToRead = true;
                if (isFilteredReading) {
                    if (subPaths.TryGetValue(structName, out var subPath)) {
                        //Если subPath нашёлся, значит его мы умеем его читать и прочитаем, если не будет ошибок
                        subPathFull = pathFromParent + "." + subPath;
                        if (!pathsList!.ContainsSubPath(subPathFull))
                            needToRead = false;
                        if (needToRead)
                            pathsList.RemoveAll(p => p == subPathFull);
                    }
                }

                switch (structName) {
                    case "CharacterSaveParameterMap":
                        if (needToRead)
                            result.CharacterSaveParameterMap = MapProperty<IndividualId, Character>.Read(
                                reader, () => IndividualId.ReadComplex(reader, messages), () => Character.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "MapObjectSaveData":
                        if (needToRead)
                            result.MapObjectSaveData = reader.ReadArrayProperty(() => MapObject.Read(reader, messages), true);
                        else
                            reader.SkipArrayProperty(size);
                        break;
                    case "FoliageGridSaveDataMap":
                        if (needToRead)
                            result.FoliageGridSaveDataMap = MapProperty<Vector3L, MapProperty<string, FoliageGridModel>>.Read(
                                reader, reader.ReadVector3LPropertyComplex,
                                () => MapProperty<string, FoliageGridModel>.ReadComplex(reader, reader.ReadString, () => FoliageGridModel.Read(reader, messages), "ModelMap"));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "MapObjectSpawnerInStageSaveData":
                        if (needToRead)
                            result.MapObjectSpawnerInStageSaveData = MapProperty<StageInstanceId, MapProperty<Guid, MapProperty<int, SpawnInfo>>>.Read(
                                reader, () => StageInstanceId.ReadComplex(reader, messages: messages), () => MapProperty<Guid, MapProperty<int, SpawnInfo>>.ReadComplex(
                                    reader, reader.ReadGuid, () => MapProperty<int, SpawnInfo>.ReadComplex(
                                        reader, reader.ReadInt32, () => SpawnInfo.Read(reader, messages), "ItemMap"), "SpawnerDataMapByLevelObjectInstanceId"));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "WorkSaveData":
                        if (needToRead)
                            result.WorkSaveData = reader.ReadArrayProperty(() => Work.Read(reader, messages), true);
                        else
                            reader.SkipArrayProperty(size);
                        break;
                    case "BaseCampSaveData":
                        if (needToRead)
                            result.BaseCampSaveData = MapProperty<Guid, BaseCamp>.Read(
                                reader, reader.ReadGuid, () => BaseCamp.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "ItemContainerSaveData":
                        if (needToRead)
                            result.ItemContainerSaveData = MapProperty<Guid, ItemContainer>.Read(
                                reader, () => StructProperty.ReadSPComplex(reader, reader.ReadGuid, "ID"), () => ItemContainer.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "DynamicItemSaveData":
                        if (needToRead)
                            result.DynamicItemSaveData = reader.ReadArrayProperty(() => DynamicItem.Read(reader, messages), true);
                        else
                            reader.SkipArrayProperty(size);
                        break;
                    case "CharacterContainerSaveData":
                        if (needToRead)
                            result.CharacterContainerSaveData = MapProperty<Guid, CharacterContainer>.Read(
                                reader, () => StructProperty.ReadSPComplex(reader, reader.ReadGuid, "ID"), () => CharacterContainer.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "GroupSaveDataMap":
                        if (needToRead)
                            result.GroupSaveDataMap = MapProperty<Guid, Group>.Read(
                                reader, reader.ReadGuid, () => Group.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "GameTimeSaveData":
                        StructPropertyHeader.Read(reader);
                        if (needToRead) {
                            var subLocalMessages = new MessageCollection();
                            var subStructName = reader.ReadString();
                            long gameTimeSaveData = 0, realDateTimeTicks = 0;
                            while (subStructName != "None") {
                                var subTypeName = reader.ReadString();
                                var SubSize = reader.ReadUInt64();

                                switch (subStructName) {
                                    case "GameDateTimeTicks":
                                        gameTimeSaveData = reader.ReadInt64Property(); break;
                                    case "RealDateTimeTicks":
                                        realDateTimeTicks = reader.ReadInt64Property(); break;
                                    default:
                                        if (messages == null)
                                            throw new InvalidDataException($"Unknown GameTimeSaveData struct {structName}");
                                        subLocalMessages.Add(new Message("StructName", "GameTimeSaveData", $"Unknown structName {structName} of type {typeName}", null));
                                        reader.Skip(size);
                                        break;
                                }

                                subStructName = reader.ReadString();
                            }
                            result.GameTimeSaveData = (gameTimeSaveData, realDateTimeTicks);
                            if (messages != null) {
                                foreach (var message in subLocalMessages) {
                                    message.Data = result.GameTimeSaveData.ToString();
                                }
                                messages.AddRange(subLocalMessages);
                            }
                        } else
                            reader.Skip(size);
                        break;
                    case "EnemyCampSaveData":
                        if (needToRead)
                            result.EnemyCampSaveData = StructProperty.ReadSP(reader, () => MapProperty<string, EnemyCampStatus>.ReadComplex(
                                reader, reader.ReadString, () => EnemyCampStatus.Read(reader, messages), "EnemyCampStatusMap"));
                        else
                            StructProperty.Skip(reader, size);
                        break;
                    case "DungeonPointMarkerSaveData":
                        if (needToRead)
                            result.DungeonPointMarkerSaveData = reader.ReadArrayProperty(() => DungeonPointMarker.Read(reader, messages), true);
                        else
                            reader.SkipArrayProperty(size);
                        break;
                    case "DungeonSaveData":
                        if (needToRead)
                            result.DungeonSaveData = reader.ReadArrayProperty(() => Dungeon.Read(reader, messages), true);
                        else
                            reader.SkipArrayProperty(size);
                        break;
                    case "BossSpawnerSaveData":
                        if (needToRead)
                            result.BossSpawnerSaveData = StructProperty.ReadSP(reader, () => MapProperty<string, bool>.ReadComplex(
                                reader, reader.ReadString, reader.ReadBoolean, "RespawnDisableFlag"));
                        else
                            StructProperty.Skip(reader, size);
                        break;
                    case "InvaderSaveData":
                        if (needToRead)
                            result.InvaderSaveData = MapProperty<Guid, InvaderData>.Read(reader, reader.ReadGuid, () => InvaderData.Read(reader, messages));
                        else
                            MapProperty<int, int>.Skip(reader, size);
                        break;
                    case "OilrigSaveData":
                        if (needToRead)
                            result.OilrigMap = StructProperty.ReadSP(reader, () => MapProperty<PalOilrigType, OilrigData>.ReadComplex(
                                reader, reader.ReadEnum<PalOilrigType>, () => OilrigData.Read(reader, messages), "OilrigMap"));
                        else
                            StructProperty.Skip(reader, size);
                        break;
                    case "SupplySaveData":
                        if (needToRead)
                            result.SupplyData = SupplySaveData.Read(reader, messages);
                        else
                            SupplySaveData.Skip(reader, size);
                        break;
                    case "WorldMetaSaveVersionBitMask":
                        if (needToRead)
                            result.WorldMetaSaveVersionBitMask = reader.ReadUInt32Property();
                        else
                            reader.SkipUInt32Property(size);
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown WorldSaveData struct {structName}");
                        localMessages.Add(new Message("StructName", "WorldSaveData", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }
                System.Diagnostics.Debug.WriteLine($"{structName};{size};{bytesLeft};{reader.BytesLeft}");
                progress?.Report(new(progressReportType, reader.WasReadPart));

                if (isFilteredReading && pathsList!.Count == 0)
                    break;

                structName = reader.ReadString();
            }

            if (isFilteredReading && reader.BytesLeft > readerBytesShouldLeft)
                reader.Skip(reader.BytesLeft - readerBytesShouldLeft);

            if (messages != null)
                messages.AddRange(localMessages);
            return result;
        }



        public static WorldSaveData Read(GvasFileReader reader, ulong size, MessageCollection? messages = null) =>
            Read(reader, size, null, messages);

        public static WorldSaveData Read(GvasFileReader reader, ulong size, IProgress<SaveReadingProgressData>? progress) =>
            Read(reader, size, progress, null);

        public static WorldSaveData Read(GvasFileReader reader, ulong size) =>
            Read(reader, size, null, null);
    }
}
