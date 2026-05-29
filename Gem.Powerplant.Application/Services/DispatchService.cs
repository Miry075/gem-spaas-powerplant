using Microsoft.Extensions.Logging;
using Gem.Powerplant.Application.Interfaces;
using Gem.Powerplant.Domain.Models;
using Gem.Powerplant.Application.DTOs;
using Gem.Powerplant.Application.Processors;
using System.Collections.Generic;
using System;


namespace Gem.Powerplant.Application.Services;

public class DispatchService : IDispatchService
{
    private readonly ILogger _logger;
    private readonly IProductionService _productionService;
    private readonly CostMarginalProcesor _costMarginalProcesor;

    public DispatchService(
        ILogger<DispatchService> logger,
        IProductionService productionService,
        CostMarginalProcesor costMarginalProcesor)
    {
        _logger = logger;
        _productionService = productionService;
        _costMarginalProcesor = costMarginalProcesor;
    }

    public IEnumerable<PowerplantProduction> DispatchProduction(ProductionPlanRequest productionPlanRequest)
    {
        try
        {
            _logger.LogInformation("Starting dispatching production for load {Load} with {PowerplantCount} powerplants",
            productionPlanRequest.Load, productionPlanRequest.Powerplants.Count);
            var meritOrderedPowerplants = productionPlanRequest
                .Powerplants
                .OrderBy(x => _costMarginalProcesor.ComputeMarginalCost(
                    x.Type,
                    x.Efficiency,
                    productionPlanRequest.Fuels));
            return _productionService.DispatchProduction(productionPlanRequest.Load,
                meritOrderedPowerplants,
                productionPlanRequest.Fuels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while dispatching production. Message: {Message}", ex.Message);
            throw;
        }
    }
}

