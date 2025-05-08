using System.Text.Json.Serialization;

namespace PalsBreedingAdvicer
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum IntervalEndpoints
    {
        Closed = 0,
        LeftOpen = 1,
        RightOpen = 2,
        Open = 3
    }
}
