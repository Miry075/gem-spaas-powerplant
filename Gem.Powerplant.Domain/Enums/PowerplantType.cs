using System.Text.Json.Serialization;

namespace Gem.Powerplant.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<PowerplantType>))]
public enum PowerplantType
{
    [JsonStringEnumMemberName("gasfired")]
    GasFired,

    [JsonStringEnumMemberName("turbojet")]
    TurboJet,

    [JsonStringEnumMemberName("windturbine")]
    WindTurbine
}
