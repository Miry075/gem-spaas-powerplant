using System.ComponentModel.Design;
using Gem.Powerplant.Application.Interfaces;
using Gem.Powerplant.Domain.Enums;
using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Processors;

public class CostMarginalProcesor
{
    private readonly Dictionary<PowerplantType, IMarginalCost> _marginalCosts;

    public CostMarginalProcesor(IEnumerable<IMarginalCost> marginalCosts)
    {
        _marginalCosts = marginalCosts.ToDictionary(mc => mc.PowerplantType, mc => mc);
    }

    public double ComputeMarginalCost(PowerplantType powerplantType, double efficiency, FuelsInfo fuelsInfo)
    {
        if (!_marginalCosts.TryGetValue(powerplantType, out var marginalCost))
        {
            throw new ArgumentException($"No marginal cost found for powerplant type: {powerplantType}");
        }
        return marginalCost.ComputeCost(efficiency, fuelsInfo);
    }
}