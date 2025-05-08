using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalSupplyType
    {
        Meteor,
        Capsule,
    }
}
