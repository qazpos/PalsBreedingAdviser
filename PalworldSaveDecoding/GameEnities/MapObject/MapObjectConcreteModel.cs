using PalworldSaveDecoding.MessageCollecting;
using System.Collections.ObjectModel;

namespace PalworldSaveDecoding
{
    public class MapObjectConcreteModel : StructProperty
    {
        //public MapProperty<string, byte[]> ModuleMapRawData { get; private set; } = new();
        public MapProperty<string, MapObjectConcreteModelModule> ModuleMap { get; private set; } = new();

        public byte[]? RawData { get; private set; }
        public bool IsAutoPickedUp { get; private set; }
        public Guid InstanceId { get; private set; }
        public Guid ModelInstanceId { get; private set; }
        public Guid StoredParameterId { get; private set; }
        public Guid OwnerPlayerId { get; private set; }
        public string? CurrentRecipeId { get; private set; }
        public int RemainProductNum { get; private set; }
        public int RequestedProductNum { get; private set; }
        public float WorkSpeedAdditionalRate { get; private set; }
        public string? ConcreteModelType { get; private set; }
        public ItemId? ItemId { get; private set; }
        public (ItemId ItemId, uint Num)[]? DropItemInfos { get; private set; }
        public int RemainingBullets { get; private set; }
        public int MagazineSize { get; private set; }
        public string? BulletItemName { get; private set; }
        public float StoredEnergyAmount { get; private set; }
        public string? CropDataId { get; private set; }
        public byte CurrentState { get; private set; }
        public float CropProgressRateValue { get; private set; }
        public float WaterStackRateValue { get; private set; }
        public (float GrowupRequiredTime, float GrowupProgressTime) StateMachine { get; private set; }
        public Guid LocationInstanceId { get; private set; }
        public int[]? ShippingHours { get; private set; }
        public string? ProductItemId { get; private set; }
        public float RecoverAmountBySec { get; private set; }
        public int UnknownBytes { get; private set; }
        public Guid HatchedCharacterGuid { get; private set; }
        public byte TreasureGradeType { get; private set; }
        public Guid[]? SpawnedEggInstanceIds { get; private set; }
        public string? SingboardText { get; private set; }
        public long ExtinctionDateTime { get; private set; }
        public Guid BaseCampId { get; private set; }

        public byte[]? CustomVersionData { get; private set; }


        private static readonly ReadOnlyDictionary<string, string> MapObjectToConcreteModel
            = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
        {
            {"droppedcharacter", "PalMapObjectDeathDroppedCharacterModel"},
            {"blastfurnace", "PalMapObjectConvertItemModel"},
            {"blastfurnace2", "PalMapObjectConvertItemModel"},
            {"blastfurnace3", "PalMapObjectConvertItemModel"},
            {"blastfurnace4", "PalMapObjectConvertItemModel"},
            {"blastfurnace5", "PalMapObjectConvertItemModel"},
            {"campfire", "PalMapObjectConvertItemModel"},
            {"characterrankup", "PalMapObjectRankUpCharacterModel"},
            {"commondropitem3d", "PalMapObjectDropItemModel"},
            {"cookingstove", "PalMapObjectConvertItemModel"},
            {"damagablerock_pv", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0001", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0002", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0003", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0004", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0005", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0017", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0006", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0007", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0008", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0009", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0010", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0011", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0012", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0013", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0014", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0015", "PalMapObjectItemDropOnDamagModel"},
            {"damagablerock0016", "PalMapObjectItemDropOnDamagModel"},
            {"deathpenaltychest", "PalMapObjectDeathPenaltyStorageModel"},
            {"defensegatlinggun", "PalMapObjectDefenseBulletLauncherModel"},
            {"defensemachinegun", "PalMapObjectDefenseBulletLauncherModel"},
            {"defenseminigun", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"defensebowgun", "PalMapObjectDefenseBulletLauncherModel"},
            {"defensemissile", "PalMapObjectDefenseBulletLauncherModel"},
            {"defensewait", "PalMapObjectDefenseWaitModel"},
            {"electricgenerator", "PalMapObjectGenerateEnergyModel"},
            {"electricgenerator_slave", "PalMapObjectGenerateEnergyModel"},
            {"electricgenerator2", "PalMapObjectGenerateEnergyModel"},
            {"electricgenerator3", "PalMapObjectGenerateEnergyModel"},
            {"electrickitchen", "PalMapObjectConvertItemModel"},
            {"factory_comfortable_01", "PalMapObjectConvertItemModel"},
            {"factory_comfortable_02", "PalMapObjectConvertItemModel"},
            {"factory_hard_01", "PalMapObjectConvertItemModel"},
            {"factory_hard_02", "PalMapObjectConvertItemModel"},
            {"factory_hard_03", "PalMapObjectConvertItemModel"},
            {"farmblockv2_grade01", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_grade02", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_grade03", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_wheet", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_tomato", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_lettuce", "PalMapObjectFarmBlockV2Model"},
            {"farmblockv2_berries", "PalMapObjectFarmBlockV2Model"},
            {"fasttravelpoint", "PalMapObjectFastTravelPointModel"},
            {"hightechkitchen", "PalMapObjectConvertItemModel"},
            {"itemchest", "PalMapObjectItemChestModel"},
            {"itemchest_02", "PalMapObjectItemChestModel"},
            {"itemchest_03", "PalMapObjectItemChestModel"},
            {"dev_itemchest", "PalMapObjectItemChestModel"},
            {"medicalpalbed", "PalMapObjectMedicalPalBedModel"},
            {"medicalpalbed_02", "PalMapObjectMedicalPalBedModel"},
            {"medicalpalbed_03", "PalMapObjectMedicalPalBedModel"},
            {"medicalpalbed_04", "PalMapObjectMedicalPalBedModel"},
            {"medicinefacility_01", "PalMapObjectConvertItemModel"},
            {"medicinefacility_02", "PalMapObjectConvertItemModel"},
            {"medicinefacility_03", "PalMapObjectConvertItemModel"},
            {"palfoodbox", "PalMapObjectPalFoodBoxModel"},
            {"palboxv2", "PalMapObjectBaseCampPoint"},
            {"displaycharacter", "PalMapObjectDisplayCharacterModel"},
            {"pickupitem_flint", "PalMapObjectPickupItemOnLevelModel"},
            {"pickupitem_log", "PalMapObjectPickupItemOnLevelModel"},
            {"pickupitem_redberry", "PalMapObjectPickupItemOnLevelModel"},
            {"pickupitem_stone", "PalMapObjectPickupItemOnLevelModel"},
            {"pickupitem_potato", "PalMapObjectPickupItemOnLevelModel"},
            {"pickupitem_poppy", "PalMapObjectPickupItemOnLevelModel"},
            {"playerbed", "PalMapObjectPlayerBedModel"},
            {"playerbed_02", "PalMapObjectPlayerBedModel"},
            {"playerbed_03", "PalMapObjectPlayerBedModel"},
            {"shippingitembox", "PalMapObjectShippingItemModel"},
            {"spherefactory_black_01", "PalMapObjectConvertItemModel"},
            {"spherefactory_black_02", "PalMapObjectConvertItemModel"},
            {"spherefactory_black_03", "PalMapObjectConvertItemModel"},
            {"spherefactory_white_01", "PalMapObjectConvertItemModel"},
            {"spherefactory_white_02", "PalMapObjectConvertItemModel"},
            {"spherefactory_white_03", "PalMapObjectConvertItemModel"},
            {"stonehouse1", "PalBuildObject"},
            {"stonepit", "PalMapObjectProductItemModel"},
            {"strawhouse1", "PalBuildObject"},
            {"weaponfactory_clean_01", "PalMapObjectConvertItemModel"},
            {"weaponfactory_clean_02", "PalMapObjectConvertItemModel"},
            {"weaponfactory_clean_03", "PalMapObjectConvertItemModel"},
            {"weaponfactory_dirty_01", "PalMapObjectConvertItemModel"},
            {"weaponfactory_dirty_02", "PalMapObjectConvertItemModel"},
            {"weaponfactory_dirty_03", "PalMapObjectConvertItemModel"},
            {"well", "PalMapObjectProductItemModel"},
            {"woodhouse1", "PalBuildObject"},
            {"workbench", "PalMapObjectConvertItemModel"},
            {"recoverotomo", "PalMapObjectRecoverOtomoModel"},
            {"palegg", "PalMapObjectPalEggModel"},
            {"palegg_fire", "PalMapObjectPalEggModel"},
            {"palegg_water", "PalMapObjectPalEggModel"},
            {"palegg_leaf", "PalMapObjectPalEggModel"},
            {"palegg_electricity", "PalMapObjectPalEggModel"},
            {"palegg_ice", "PalMapObjectPalEggModel"},
            {"palegg_earth", "PalMapObjectPalEggModel"},
            {"palegg_dark", "PalMapObjectPalEggModel"},
            {"palegg_dragon", "PalMapObjectPalEggModel"},
            {"hatchingpalegg", "PalMapObjectHatchingEggModel"},
            {"treasurebox", "PalMapObjectTreasureBoxModel"},
            {"treasurebox_visiblecontent", "PalMapObjectPickupItemOnLevelModel"},
            {"treasurebox_visiblecontent_skillfruits", "PalMapObjectPickupItemOnLevelModel"},
            {"stationdeforest2", "PalMapObjectProductItemModel"},
            {"workbench_skillunlock", "PalMapObjectConvertItemModel"},
            {"workbench_skillcard", "PalMapObjectConvertItemModel"},
            {"wooden_foundation", "PalBuildObject"},
            {"wooden_wall", "PalBuildObject"},
            {"wooden_roof", "PalBuildObject"},
            {"wooden_stair", "PalBuildObject"},
            {"wooden_doorwall", "PalMapObjectDoorModel"},
            {"stone_foundation", "PalBuildObject"},
            {"stone_wall", "PalBuildObject"},
            {"stone_roof", "PalBuildObject"},
            {"stone_stair", "PalBuildObject"},
            {"stone_doorwall", "PalMapObjectDoorModel"},
            {"metal_foundation", "PalBuildObject"},
            {"metal_wall", "PalBuildObject"},
            {"metal_roof", "PalBuildObject"},
            {"metal_stair", "PalBuildObject"},
            {"metal_doorwall", "PalMapObjectDoorModel"},
            {"buildablegoddessstatue", "PalMapObjectCharacterStatusOperatorModel"},
            {"spa", "PalMapObjectAmusementModel"},
            {"spa2", "PalMapObjectAmusementModel"},
            {"pickupitem_mushroom", "PalMapObjectPickupItemOnLevelModel"},
            {"defensewall_wood", "PalBuildObject"},
            {"defensewall", "PalBuildObject"},
            {"defensewall_metal", "PalBuildObject"},
            {"heater", "PalMapObjectHeatSourceModel"},
            {"electricheater", "PalMapObjectHeatSourceModel"},
            {"cooler", "PalMapObjectHeatSourceModel"},
            {"electriccooler", "PalMapObjectHeatSourceModel"},
            {"torch", "PalMapObjectTorchModel"},
            {"walltorch", "PalMapObjectTorchModel"},
            {"lamp", "PalMapObjectLampModel"},
            {"ceilinglamp", "PalMapObjectLampModel"},
            {"largelamp", "PalMapObjectLampModel"},
            {"largeceilinglamp", "PalMapObjectLampModel"},
            {"crusher", "PalMapObjectConvertItemModel"},
            {"woodcrusher", "PalMapObjectConvertItemModel"},
            {"flourmill", "PalMapObjectConvertItemModel"},
            {"trap_leghold", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_leghold_big", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_noose", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_movingpanel", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_mineelecshock", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_minefreeze", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"trap_mineattack", "DEFAULT_UNKNOWN_PalMapObjectConcreteModelBase"},
            {"breedfarm", "PalMapObjectBreedFarmModel"},
            {"wood_gate", "PalMapObjectDoorModel"},
            {"stone_gate", "PalMapObjectDoorModel"},
            {"metal_gate", "PalMapObjectDoorModel"},
            {"repairbench", "PalMapObjectRepairItemModel"},
            {"skillfruit_test", "PalMapObjectPickupItemOnLevelModel"},
            {"toolboxv1", "PalMapObjectBaseCampPassiveEffectModel"},
            {"toolboxv2", "PalMapObjectBaseCampPassiveEffectModel"},
            {"fountain", "PalMapObjectBaseCampPassiveEffectModel"},
            {"silo", "PalMapObjectBaseCampPassiveEffectModel"},
            {"transmissiontower", "PalMapObjectBaseCampPassiveEffectModel"},
            {"flowerbed", "PalMapObjectBaseCampPassiveEffectModel"},
            {"stump", "PalMapObjectBaseCampPassiveEffectModel"},
            {"miningtool", "PalMapObjectBaseCampPassiveEffectModel"},
            {"cauldron", "PalMapObjectBaseCampPassiveEffectModel"},
            {"snowman", "PalMapObjectBaseCampPassiveEffectModel"},
            {"olympiccauldron", "PalMapObjectBaseCampPassiveEffectModel"},
            {"basecampworkhard", "PalMapObjectBaseCampPassiveWorkHardModel"},
            {"coolerbox", "PalMapObjectItemChest_AffectCorruption"},
            {"refrigerator", "PalMapObjectItemChest_AffectCorruption"},
            {"damagedscarecrow", "PalMapObjectDamagedScarecrowModel"},
            {"signboard", "PalMapObjectSignboardModel"},
            {"basecampbattledirector", "PalMapObjectBaseCampWorkerDirectorModel"},
            {"monsterfarm", "PalMapObjectMonsterFarmModel"},
            {"wood_windowwall", "PalBuildObject"},
            {"stone_windowwall", "PalBuildObject"},
            {"metal_windowwall", "PalBuildObject"},
            {"wood_trianglewall", "PalBuildObject"},
            {"stone_trianglewall", "PalBuildObject"},
            {"metal_trianglewall", "PalBuildObject"},
            {"wood_slantedroof", "PalBuildObject"},
            {"stone_slantedroof", "PalBuildObject"},
            {"metal_slantedroof", "PalBuildObject"},
            {"table1", "PalBuildObject"},
            {"barrel_wood", "PalMapObjectItemChestModel"},
            {"box_wood", "PalMapObjectItemChestModel"},
            {"box01_iron", "PalMapObjectItemChestModel"},
            {"box02_iron", "PalMapObjectItemChestModel"},
            {"shelf_wood", "PalMapObjectItemChestModel"},
            {"shelf_cask_wood", "PalMapObjectItemChestModel"},
            {"shelf_hang01_wood", "PalMapObjectItemChestModel"},
            {"shelf01_iron", "PalMapObjectItemChestModel"},
            {"shelf02_iron", "PalMapObjectItemChestModel"},
            {"shelf03_iron", "PalMapObjectItemChestModel"},
            {"shelf04_iron", "PalMapObjectItemChestModel"},
            {"shelf05_stone", "PalMapObjectItemChestModel"},
            {"shelf06_stone", "PalMapObjectItemChestModel"},
            {"shelf07_stone", "PalMapObjectItemChestModel"},
            {"shelf01_wall_stone", "PalMapObjectItemChestModel"},
            {"shelf01_wall_iron", "PalMapObjectItemChestModel"},
            {"shelf01_stone", "PalMapObjectItemChestModel"},
            {"shelf02_stone", "PalMapObjectItemChestModel"},
            {"shelf03_stone", "PalMapObjectItemChestModel"},
            {"shelf04_stone", "PalMapObjectItemChestModel"},
            {"container01_iron", "PalMapObjectItemChestModel"},
            {"tablesquare_wood", "PalBuildObject"},
            {"tablecircular_wood", "PalBuildObject"},
            {"bench_wood", "PalBuildObject"},
            {"stool_wood", "PalBuildObject"},
            {"decal_palsticker_pinkcat", "PalBuildObject"},
            {"stool_high_wood", "PalBuildObject"},
            {"counter_wood", "PalBuildObject"},
            {"rug_wood", "PalBuildObject"},
            {"shelf_hang02_wood", "PalBuildObject"},
            {"ivy01", "PalBuildObject"},
            {"ivy02", "PalBuildObject"},
            {"ivy03", "PalBuildObject"},
            {"chair01_wood", "PalBuildObject"},
            {"box01_stone", "PalBuildObject"},
            {"barrel01_iron", "PalBuildObject"},
            {"barrel02_iron", "PalBuildObject"},
            {"barrel03_iron", "PalBuildObject"},
            {"cablecoil01_iron", "PalBuildObject"},
            {"chair01_iron", "PalBuildObject"},
            {"chair02_iron", "PalBuildObject"},
            {"clock01_wall_iron", "PalBuildObject"},
            {"garbagebag_iron", "PalBuildObject"},
            {"goalsoccer_iron", "PalBuildObject"},
            {"machinegame01_iron", "PalBuildObject"},
            {"machinevending01_iron", "PalBuildObject"},
            {"pipeclay01_iron", "PalBuildObject"},
            {"signexit_ceiling_iron", "PalBuildObject"},
            {"signexit_wall_iron", "PalBuildObject"},
            {"sofa01_iron", "PalBuildObject"},
            {"sofa02_iron", "PalBuildObject"},
            {"stool01_iron", "PalBuildObject"},
            {"tablecircular01_iron", "PalBuildObject"},
            {"tableside01_iron", "PalBuildObject"},
            {"tablesquare01_iron", "PalBuildObject"},
            {"tablesquare02_iron", "PalBuildObject"},
            {"tire01_iron", "PalBuildObject"},
            {"trafficbarricade01_iron", "PalBuildObject"},
            {"trafficbarricade02_iron", "PalBuildObject"},
            {"trafficbarricade03_iron", "PalBuildObject"},
            {"trafficbarricade04_iron", "PalBuildObject"},
            {"trafficbarricade05_iron", "PalBuildObject"},
            {"trafficcone01_iron", "PalBuildObject"},
            {"trafficcone02_iron", "PalBuildObject"},
            {"trafficcone03_iron", "PalBuildObject"},
            {"trafficlight01_iron", "PalBuildObject"},
            {"bathtub_stone", "PalBuildObject"},
            {"chair01_stone", "PalBuildObject"},
            {"chair02_stone", "PalBuildObject"},
            {"clock01_stone", "PalBuildObject"},
            {"curtain01_wall_stone", "PalBuildObject"},
            {"desk01_stone", "PalBuildObject"},
            {"globe01_stone", "PalBuildObject"},
            {"mirror01_stone", "PalBuildObject"},
            {"mirror02_stone", "PalBuildObject"},
            {"mirror01_wall_stone", "PalBuildObject"},
            {"partition_stone", "PalBuildObject"},
            {"piano01_stone", "PalBuildObject"},
            {"piano02_stone", "PalBuildObject"},
            {"rug01_stone", "PalBuildObject"},
            {"rug02_stone", "PalBuildObject"},
            {"rug03_stone", "PalBuildObject"},
            {"rug04_stone", "PalBuildObject"},
            {"sofa01_stone", "PalBuildObject"},
            {"sofa02_stone", "PalBuildObject"},
            {"sofa03_stone", "PalBuildObject"},
            {"stool01_stone", "PalBuildObject"},
            {"stove01_stone", "PalBuildObject"},
            {"tablecircular01_stone", "PalBuildObject"},
            {"tabledresser01_stone", "PalBuildObject"},
            {"tablesink01_stone", "PalBuildObject"},
            {"toilet01_stone", "PalBuildObject"},
            {"toiletholder01_stone", "PalBuildObject"},
            {"towlrack01_stone", "PalBuildObject"},
            {"plant01_plant", "PalBuildObject"},
            {"plant02_plant", "PalBuildObject"},
            {"plant03_plant", "PalBuildObject"},
            {"plant04_plant", "PalBuildObject"},
            {"light_floorlamp01", "PalMapObjectLampModel"},
            {"light_floorlamp02", "PalMapObjectLampModel"},
            {"light_lightpole01", "PalMapObjectLampModel"},
            {"light_lightpole02", "PalMapObjectLampModel"},
            {"light_lightpole03", "PalMapObjectLampModel"},
            {"light_lightpole04", "PalMapObjectLampModel"},
            {"light_fireplace01", "PalMapObjectTorchModel"},
            {"light_fireplace02", "PalMapObjectTorchModel"},
            {"light_candlesticks_top", "PalMapObjectLampModel"},
            {"light_candlesticks_wall", "PalMapObjectLampModel"},
            {"television01_iron", "PalBuildObject"},
            {"desk01_iron", "PalBuildObject"},
            {"trafficsign01_iron", "PalBuildObject"},
            {"trafficsign02_iron", "PalBuildObject"},
            {"trafficsign03_iron", "PalBuildObject"},
            {"trafficsign04_iron", "PalBuildObject"},
            {"chair01_pal", "PalBuildObject"},
        });

        private static readonly ReadOnlyCollection<string> NoOpTypes
            = new ReadOnlyCollection<string>(new List<string>()
            {
                "Default_PalMapObjectConcreteModelBase",
                "PalBuildObject",
                "PalMapObjectRankUpCharacterModel",
                "PalMapObjectDefenseWaitModel",
                "PalMapObjectItemChestModel",
                "PalMapObjectMedicalPalBedModel",
                "PalMapObjectPalFoodBoxModel",
                "PalMapObjectPlayerBedModel",
                "PalMapObjectDisplayCharacterModel",
                "PalMapObjectDoorModel",
                "PalMapObjectCharacterStatusOperatorModel",
                "PalMapObjectAmusementModel",
                "PalMapObjectRepairItemModel",
                "PalMapObjectBaseCampPassiveEffectModel",
                "PalMapObjectBaseCampPassiveWorkHardModel",
                "PalMapObjectItemChest_AffectCorruption",
                "PalMapObjectDamagedScarecrowModel",
                "PalMapObjectBaseCampWorkerDirectorModel",
                "PalMapObjectMonsterFarmModel",
                "PalMapObjectLampModel",
                "PalMapObjectHeatSourceModel",
            });




        public static MapObjectConcreteModel Read(GvasFileReader reader, string mapObjectId, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObjectConcreteModel();
            result.Header = StructPropertyHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "ModuleMap":
                        //result.ModuleMapRawData = MapProperty<string, byte[]>.Read(reader, reader.ReadString,
                        //    () => reader.ReadArrayPropertyComplex(reader.ReadByte, "RawData"));
                        //result.DecodeModuleMap(result.ModuleMapRawData);
                        result.ModuleMap = MapProperty<string, MapObjectConcreteModelModule>.Read(reader, reader.ReadString,
                            (k) => MapObjectConcreteModelModule.Read(reader, k, messages));
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData.ToArray(), mapObjectId, messages);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObjectConcreteModel struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectConcreteModel", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecodeRawData(byte[] data, string mapObjectId, MessageCollection? messages = null)
        {
            if (!MapObjectToConcreteModel.Keys.Contains(mapObjectId))
                return;

            mapObjectId = mapObjectId.ToLower();
            ConcreteModelType = MapObjectToConcreteModel[mapObjectId];

            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                InstanceId = reader.ReadGuid();
                ModelInstanceId = reader.ReadGuid();

                if (NoOpTypes.Contains(ConcreteModelType))
                    return;

                switch (ConcreteModelType)
                {
                    case "PalMapObjectDeathDroppedCharacterModel":
                        StoredParameterId = reader.ReadGuid();
                        OwnerPlayerId = reader.ReadGuid();
                        break;
                    case "PalMapObjectConvertItemModel":
                        CurrentRecipeId = reader.ReadString();
                        RemainProductNum = reader.ReadInt32();
                        RequestedProductNum = reader.ReadInt32();
                        WorkSpeedAdditionalRate = reader.ReadFloat();
                        break;
                    case "PalMapObjectPickupItemOnLevelModel":
                        IsAutoPickedUp = reader.ReadUInt32() > 0;
                        break;
                    case "PalMapObjectDropItemModel":
                        IsAutoPickedUp = reader.ReadUInt32() > 0;
                        ItemId = ItemId.Read(reader);
                        break;
                    case "PalMapObjectItemDropOnDamagModel":
                        DropItemInfos = reader.ReadArray(() => (ItemId.Read(reader), reader.ReadUInt32()));
                        break;
                    case "PalMapObjectDeathPenaltyStorageModel":
                        OwnerPlayerId = reader.ReadGuid();
                        break;
                    case "PalMapObjectDefenseBulletLauncherModel":
                        RemainingBullets = reader.ReadInt32();
                        MagazineSize = reader.ReadInt32();
                        BulletItemName = reader.ReadString();
                        break;
                    case "PalMapObjectGenerateEnergyModel":
                        StoredEnergyAmount = reader.ReadFloat();
                        break;
                    case "PalMapObjectFarmBlockV2Model":
                        CropDataId = reader.ReadString();
                        CurrentState = reader.ReadByte();
                        CropProgressRateValue = reader.ReadFloat();
                        WaterStackRateValue = reader.ReadFloat();

                        if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                            StateMachine = (reader.ReadFloat(), reader.ReadFloat());
                        break;
                    case "PalMapObjectFastTravelPointModel":
                        LocationInstanceId = reader.ReadGuid(); break;
                    case "PalMapObjectShippingItemModel":
                        ShippingHours = reader.ReadArray(reader.ReadInt32); break;
                    case "PalMapObjectProductItemModel":
                        WorkSpeedAdditionalRate = reader.ReadFloat();
                        ProductItemId = reader.ReadString();
                        break;
                    case "PalMapObjectRecoverOtomoModel":
                        RecoverAmountBySec = reader.ReadFloat(); break;
                    case "PalMapObjectHatchingEggModel":
                        ReadMapObjectHatchingEggModel(reader, messages);
                        UnknownBytes = reader.ReadInt32();
                        HatchedCharacterGuid = reader.ReadGuid();
                        break;
                    case "PalMapObjectTreasureBoxModel":
                        TreasureGradeType = reader.ReadByte(); break;
                    case "PalMapObjectBreedFarmModel":
                        SpawnedEggInstanceIds = reader.ReadArray(reader.ReadGuid); break;
                    case "PalMapObjectSignboardModel":
                        SingboardText = reader.ReadString(); break;
                    case "PalMapObjectTorchModel":
                        ExtinctionDateTime = reader.ReadInt64(); break;
                    case "PalMapObjectPalEggModel":
                        UnknownBytes = reader.ReadInt32(); break;
                    case "PalMapObjectBaseCampPoint":
                        BaseCampId = reader.ReadGuid(); break;
                    default:
                        RawData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)); break;
                } 
            }
        }

        //private void DecodeModuleMap(MapProperty<string, byte[]> data)
        //{
        //    foreach (var moduleRawData in data)
        //        using (var reader = new GvasFileReader(new MemoryStream(moduleRawData.Value), true))
        //            ModuleMap.Add(moduleRawData.Key, MapObjectConcreteModelModule.Read(reader, moduleRawData.Key));
        //}



        private void ReadMapObjectHatchingEggModel(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObjectConcreteModel.HatchingEggModel struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObjectConcreteModel.HatchingEggModel", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = ToString();
                }
                messages.AddRange(localMessages);
            }
        }
    }
}
