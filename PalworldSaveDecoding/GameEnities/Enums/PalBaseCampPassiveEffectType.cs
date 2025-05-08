using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalBaseCampPassiveEffectType
    {
        None,
        WorkSuitability,
        WorkHard,
    }
}
