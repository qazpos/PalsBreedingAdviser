using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalWorkSuitability
    {
        EmitFlame,
        Watering,
        Seeding,
        GenerateElectricity,
        Handcraft,
        Collection,
        Deforest,
        Mining,
        OilExtraction,
        ProductMedicine,
        Cool,
        Transport,
        MonsterFarm,
    }
}
