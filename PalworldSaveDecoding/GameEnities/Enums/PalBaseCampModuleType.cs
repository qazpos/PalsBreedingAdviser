using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalBaseCampModuleType
    {
        Energy,
        Medical,
        TransportItemDirector,
        ResourceCollector,
        ItemStorages,
        FacilityReservation,
        ObjectMaintenance,
        PassiveEffect
    }
}
