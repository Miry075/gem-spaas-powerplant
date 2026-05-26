using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Interfaces;

public interface IProductionService
{
    IEnumerable<PowerplantProduction> DispatchProduction(double load,
        IEnumerable<Gem.Powerplant.Domain.Models.PowerplantModel> powerplants,
        FuelsInfo fuelsInfo);
}
