using Gem.Powerplant.Application.Interfaces;
using Gem.Powerplant.Domain.Enums;
using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Services.MarginalCosts;

public class WindTurbineCost : IMarginalCost
{
    public PowerplantType PowerplantType => PowerplantType.WindTurbine;
    public double ComputeCost(double efficiency, FuelsInfo fuelsInfo) => 0;
}