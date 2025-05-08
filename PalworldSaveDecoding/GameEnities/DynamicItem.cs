using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class DynamicItem
    {
        public DynamicId? DynamicItemId { get; private set; }
        public string? StaticItemId { get; private set; }

        public byte[]? RawData { get; private set; }
        public ItemId? Id { get; private set; }
        public string? Type { get; private set; }

        public float Durability { get; private set; }

        public string? CharacterId { get; private set; }
        public Character? EggCharacter { get; private set; }

        public int RemainingBullets { get; private set; }
        public string[]? PassiveSkillsList { get; private set; }

        public byte[]? CustomVersionData { get; private set; }


        private static List<string> eggIds = new() {
            "PalEgg_Dark_01",
            "PalEgg_Dark_02",
            "PalEgg_Dark_03",
            "PalEgg_Dark_04",
            "PalEgg_Dark_05",
            "PalEgg_Dragon_01",
            "PalEgg_Dragon_02",
            "PalEgg_Dragon_03",
            "PalEgg_Dragon_04",
            "PalEgg_Dragon_05",
            "PalEgg_Earth_01",
            "PalEgg_Earth_02",
            "PalEgg_Earth_03",
            "PalEgg_Earth_04",
            "PalEgg_Earth_05",
            "PalEgg_Electricity_01",
            "PalEgg_Electricity_02",
            "PalEgg_Electricity_03",
            "PalEgg_Electricity_04",
            "PalEgg_Electricity_05",
            "PalEgg_Fire_01",
            "PalEgg_Fire_02",
            "PalEgg_Fire_03",
            "PalEgg_Fire_04",
            "PalEgg_Fire_05",
            "PalEgg_Ice_01",
            "PalEgg_Ice_02",
            "PalEgg_Ice_03",
            "PalEgg_Ice_04",
            "PalEgg_Ice_05",
            "PalEgg_Leaf_01",
            "PalEgg_Leaf_02",
            "PalEgg_Leaf_03",
            "PalEgg_Leaf_04",
            "PalEgg_Leaf_05",
            "PalEgg_Normal_01",
            "PalEgg_Normal_02",
            "PalEgg_Normal_03",
            "PalEgg_Normal_04",
            "PalEgg_Normal_05",
            "PalEgg_Water_01",
            "PalEgg_Water_02",
            "PalEgg_Water_03",
            "PalEgg_Water_04",
            "PalEgg_Water_05",
        };




        public static DynamicItem Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new DynamicItem();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "ID":
                        result.DynamicItemId = DynamicId.ReadComplex(reader, messages); break;
                    case "StaticItemId":
                        result.StaticItemId = reader.ReadStringProperty(); break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData, messages);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown DynamicItem struct {structName}");
                        localMessages.Add(new Message("StructName", "DynamicItem", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecodeRawData(byte[] data, MessageCollection? messages = null)
        {
            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                Id = ItemId.ReadReverse(reader);
                Type = "Unknown";

                if (reader.BytesLeft == 4) {
                    Type = "Armor";
                    Durability = reader.ReadFloat();
                } else if (Id.StaticId == null)
                    throw new InvalidDataException("Unknown DynamicItem structure. Reading Egg info without ItemId.StaticId");
                else if (eggIds.Contains(Id.StaticId)) {
                    Type = "Egg";
                    CharacterId = reader.ReadString();

                    EggCharacter = Character.ReadClear(reader, messages);
                } else {
                    Durability = reader.ReadFloat();
                    RemainingBullets = reader.ReadInt32();
                    PassiveSkillsList = reader.ReadArray(reader.ReadString);
                }

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("DynamicItem raw data invalid length");
            }
        }
    }
}
