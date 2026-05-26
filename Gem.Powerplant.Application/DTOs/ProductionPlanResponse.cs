using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.DTOs;

public record ProductionPlanResponse(IEnumerable<PowerplantProduction> PowerplantProduction)
{
}

