using System;

namespace Gem.Powerplant.Domain.Models;

public record ProductionPlanResponse(IEnumerable<PowerplantProduction> PowerplantProduction)
{
}

