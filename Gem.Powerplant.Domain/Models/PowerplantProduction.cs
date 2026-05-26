using System;
using System.Text.Json.Serialization;

namespace Gem.Powerplant.Domain.Models;

public record PowerplantProduction(
	[property: JsonPropertyName("name")] string powerplantName,
	[property: JsonPropertyName("p")] double production)
{

}

