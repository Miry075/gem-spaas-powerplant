using System;

namespace Gem.Powerplant.Domain.Models;

public record ProductionPlanRequest(double Load, FuelsInfo Fuels, List<Powerplant> Powerplants)
{
}

