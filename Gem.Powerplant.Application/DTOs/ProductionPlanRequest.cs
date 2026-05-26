using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.DTOs;

public record ProductionPlanRequest(
    double Load,
    FuelsInfo Fuels,
    List<PowerplantModel> Powerplants)
{
}

