using Gem.Powerplant.Application.Interfaces;
using Gem.Powerplant.Domain.Enums;
using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Services.MarginalCosts;

public class GasFiredCost : IMarginalCost
{
    public PowerplantType PowerplantType => PowerplantType.GasFired;
    public double ComputeCost(double efficiency, FuelsInfo fuelsInfo)
    {
        ArgumentNullException.ThrowIfNull(fuelsInfo);
        return fuelsInfo.Gas / efficiency;
    }
}