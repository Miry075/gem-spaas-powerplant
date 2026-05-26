using System;
using Gem.Powerplant.Application.DTOs;
using Gem.Powerplant.Domain.Models;

namespace Gem.Powerplant.Application.Interfaces;

public interface IDispatchService
{
    IEnumerable<PowerplantProduction> DispatchProduction(ProductionPlanRequest productionPlanRequest);
}

