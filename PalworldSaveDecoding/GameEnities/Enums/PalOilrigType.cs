using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalOilrigType
    {
        TypeA,
        TypeB,
        TypeC,
        Debug
    }
}
