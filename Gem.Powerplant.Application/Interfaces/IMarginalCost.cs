using Gem.Powerplant.Domain.Enums;
using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Interfaces;

public interface IMarginalCost
{
    PowerplantType PowerplantType { get; }
    double ComputeCost(double efficiency, FuelsInfo fuelsInfo);
}