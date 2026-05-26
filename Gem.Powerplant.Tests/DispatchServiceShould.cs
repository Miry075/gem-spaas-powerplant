using System.Linq;
using Xunit;
using Gem.Powerplant.Application.Services;
using Gem.Powerplant.Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Gem.Powerplant.Application.DTOs;
using Gem.Powerplant.Domain.Enums;
using Gem.Powerplant.Application.Processors;
using Gem.Powerplant.Application.Services.MarginalCosts;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Gem.Powerplant.Application.Interfaces;



namespace Gem.Powerplant.Tests;

public class DispatchServiceShould
{

    private readonly List<IMarginalCost> _costs = new() { new GasFiredCost(), new TurboJetCost(), new WindTurbineCost() };
    private readonly DispatchService _service;

    public DispatchServiceShould()
    {
        _service = new DispatchService(
            NullLogger<DispatchService>.Instance,
            new ProductionService(NullLogger<ProductionService>.Instance),
            new CostMarginalProcesor(_costs));
    }

    [Fact]
    public void DispatchProduction_WithGivenPayload_ReturnExpectedPlan()
    {
        var request = new ProductionPlanRequest(
            480,
            new FuelsInfo(13.4, 50.8, 20, 60),
            new()
            {
                new PowerplantModel { Name = "gasfiredbig1", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredbig2", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredsomewhatsmaller", Type = PowerplantType.GasFired, Efficiency = 0.37, PMin = 40, PMax = 210 },
                new PowerplantModel { Name = "tj1", Type = PowerplantType.TurboJet, Efficiency = 0.3, PMin = 0, PMax = 16 },
                new PowerplantModel { Name = "windpark1", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 150 },
                new PowerplantModel { Name = "windpark2", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 36 },
            });

        var result = _service.DispatchProduction(request).ToList();

        Assert.Equal(6, result.Count);

        Assert.Equal("windpark1", result[0].powerplantName);
        Assert.Equal(90, result[0].production);

        Assert.Equal("windpark2", result[1].powerplantName);
        Assert.Equal(21.6, result[1].production);

        Assert.Equal("gasfiredbig1", result[2].powerplantName);
        Assert.Equal(368.4, result[2].production);

        Assert.Equal("gasfiredbig2", result[3].powerplantName);
        Assert.Equal(0, result[3].production);

        Assert.Equal("gasfiredsomewhatsmaller", result[4].powerplantName);
        Assert.Equal(0, result[4].production);

        Assert.Equal("tj1", result[5].powerplantName);
        Assert.Equal(0, result[5].production);

        Assert.Equal(480, result.Sum(x => x.production));
    }

    [Fact]
    public void DispatchProduction_WithZeroWindPayload_ReturnExpectedPlan()
    {
        var request = new Application.DTOs.ProductionPlanRequest(
            480,
            new FuelsInfo(13.4, 50.8, 20, 0),
            new()
            {
                new PowerplantModel { Name = "gasfiredbig1", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredbig2", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredsomewhatsmaller", Type = PowerplantType.GasFired, Efficiency = 0.37, PMin = 40, PMax = 210 },
                new PowerplantModel { Name = "tj1", Type = PowerplantType.TurboJet, Efficiency = 0.3, PMin = 0, PMax = 16 },
                new PowerplantModel { Name = "windpark1", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 150 },
                new PowerplantModel { Name = "windpark2", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 36 },
            });

        var result = _service.DispatchProduction(request).ToList();

        Assert.Equal(6, result.Count);

        Assert.Equal("windpark1", result[0].powerplantName);
        Assert.Equal(0, result[0].production);

        Assert.Equal("windpark2", result[1].powerplantName);
        Assert.Equal(0, result[1].production);

        Assert.Equal("gasfiredbig1", result[2].powerplantName);
        Assert.Equal(380, result[2].production);

        Assert.Equal("gasfiredbig2", result[3].powerplantName);
        Assert.Equal(100, result[3].production);

        Assert.Equal("gasfiredsomewhatsmaller", result[4].powerplantName);
        Assert.Equal(0, result[4].production);

        Assert.Equal("tj1", result[5].powerplantName);
        Assert.Equal(0, result[5].production);

        Assert.Equal(480, result.Sum(x => x.production));
    }

    [Fact]
    public void DispatchProduction_WithHighLoadPayload_ReturnExpectedPlan()
    {
        var request = new ProductionPlanRequest(
            910,
            new FuelsInfo(13.4, 50.8, 20, 60),
            new()
            {
                new PowerplantModel { Name = "gasfiredbig1", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredbig2", Type = PowerplantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 460 },
                new PowerplantModel { Name = "gasfiredsomewhatsmaller", Type = PowerplantType.GasFired, Efficiency = 0.37, PMin = 40, PMax = 210 },
                new PowerplantModel { Name = "tj1", Type = PowerplantType.TurboJet, Efficiency = 0.3, PMin = 0, PMax = 16 },
                new PowerplantModel { Name = "windpark1", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 150 },
                new PowerplantModel { Name = "windpark2", Type = PowerplantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 36 },
            });

        var result = _service.DispatchProduction(request).ToList();

        Assert.Equal(6, result.Count);

        Assert.Equal("windpark1", result[0].powerplantName);
        Assert.Equal(90, result[0].production);

        Assert.Equal("windpark2", result[1].powerplantName);
        Assert.Equal(21.6, result[1].production);

        Assert.Equal("gasfiredbig1", result[2].powerplantName);
        Assert.Equal(460, result[2].production);

        Assert.Equal("gasfiredbig2", result[3].powerplantName);
        Assert.Equal(338.4, result[3].production);

        Assert.Equal("gasfiredsomewhatsmaller", result[4].powerplantName);
        Assert.Equal(0, result[4].production);

        Assert.Equal("tj1", result[5].powerplantName);
        Assert.Equal(0, result[5].production);

        Assert.Equal(910, result.Sum(x => x.production));
    }
}