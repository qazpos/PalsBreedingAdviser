using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalGenderType
    {
        Male = 1,
        Female = 2,
    }
}
