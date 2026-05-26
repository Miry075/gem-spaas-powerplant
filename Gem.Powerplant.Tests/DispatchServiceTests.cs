// using Xunit;
// using Moq;
// using Gem.Powerplant.Application.DTOs;
// using Gem.Powerplant.Application.Interfaces;
// using Gem.Powerplant.Application.Services;
// using Gem.Powerplant.Domain.Models;
// using Gem.Powerplant.Domain.Enums;
// using Microsoft.Extensions.Logging;

// namespace Gem.Powerplant.Tests;

// public class DispatchServiceTests
// {
//     private readonly IDispatchService _dispatchService;
//     private readonly Mock<IProductionService> _productionServiceMock;
//     private readonly Mock<ILogger<DispatchService>> _loggerMock;

//     public DispatchServiceTests()
//     {
//         _loggerMock = new Mock<ILogger<DispatchService>>();
//         _productionServiceMock = new Mock<IProductionService>();
//         _dispatchService = new DispatchService(_loggerMock.Object, _productionServiceMock.Object);
//     }

//     [Fact]
//     public void DispatchProduction_WithLoad910AndWind60Percent_ShouldDispatchSuccessfully()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig2",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredsomewhatsmaller",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.37,
//                 PMin = 40,
//                 PMax = 210
//             },
//             new PowerplantModel
//             {
//                 Name = "tj1",
//                 Type = PowerplantType.TurboJet,
//                 Efficiency = 0.3,
//                 PMin = 0,
//                 PMax = 16
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark1",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 150
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark2",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 36
//             }
//         };

//         var request = new ProductionPlanRequest(910, fuels, powerplants);

//         var expectedProduction = new List<PowerplantProduction>
//         {
//             new PowerplantProduction { powerplantName = "windpark1", production = 150 },
//             new PowerplantProduction { powerplantName = "windpark2", production = 36 },
//             new PowerplantProduction { powerplantName = "gasfiredbig1", production = 460 },
//             new PowerplantProduction { powerplantName = "gasfiredbig2", production = 264 }
//         };

//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Returns(expectedProduction);

//         // Act
//         var result = _dispatchService.DispatchProduction(request).ToList();

//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(4, result.Count);
//         Assert.Equal(910, result.Sum(p => p.production));

//         _productionServiceMock.Verify(
//             x => x.DispatchProduction(910, It.IsAny<IEnumerable<PowerplantModel>>(), fuels),
//             Times.Once);
//     }

//     [Fact]
//     public void DispatchProduction_ShouldOrderPowerplantsByMeritOrder()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig2",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredsomewhatsmaller",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.37,
//                 PMin = 40,
//                 PMax = 210
//             },
//             new PowerplantModel
//             {
//                 Name = "tj1",
//                 Type = PowerplantType.TurboJet,
//                 Efficiency = 0.3,
//                 PMin = 0,
//                 PMax = 16
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark1",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 150
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark2",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 36
//             }
//         };

//         var request = new ProductionPlanRequest(910, fuels, powerplants);

//         var capturedOrderedPowerplants = new List<PowerplantModel>();
//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Callback<double, IEnumerable<PowerplantModel>, FuelsInfo>((load, plants, fuelInfo) =>
//             {
//                 capturedOrderedPowerplants.AddRange(plants);
//             })
//             .Returns(new List<PowerplantProduction>());

//         // Act
//         _dispatchService.DispatchProduction(request);

//         // Assert
//         Assert.NotNull(capturedOrderedPowerplants);
//         Assert.Equal(6, capturedOrderedPowerplants.Count);

//         // Wind turbines should come first (cost = 0)
//         Assert.Equal(PowerplantType.WindTurbine, capturedOrderedPowerplants.First().Type);
//     }

//     [Fact]
//     public void DispatchProduction_ShouldMeetTotalLoadRequirement()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig2",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark1",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 150
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark2",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 36
//             }
//         };

//         var request = new ProductionPlanRequest(910, fuels, powerplants);

//         var expectedProduction = new List<PowerplantProduction>
//         {
//             new PowerplantProduction { powerplantName = "windpark1", production = 150 },
//             new PowerplantProduction { powerplantName = "windpark2", production = 36 },
//             new PowerplantProduction { powerplantName = "gasfiredbig1", production = 460 },
//             new PowerplantProduction { powerplantName = "gasfiredbig2", production = 264 }
//         };

//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Returns(expectedProduction);

//         // Act
//         var result = _dispatchService.DispatchProduction(request).ToList();

//         // Assert
//         var totalProduction = result.Sum(p => p.production);
//         Assert.Equal(910, totalProduction);
//     }

//     [Fact]
//     public void DispatchProduction_WithAllPowerplantTypes_ShouldIncludeAllInDispatch()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig2",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "gasfiredsomewhatsmaller",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.37,
//                 PMin = 40,
//                 PMax = 210
//             },
//             new PowerplantModel
//             {
//                 Name = "tj1",
//                 Type = PowerplantType.TurboJet,
//                 Efficiency = 0.3,
//                 PMin = 0,
//                 PMax = 16
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark1",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 150
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark2",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 36
//             }
//         };

//         var request = new ProductionPlanRequest(910, fuels, powerplants);

//         var expectedProduction = new List<PowerplantProduction>
//         {
//             new PowerplantProduction { powerplantName = "windpark1", production = 150 },
//             new PowerplantProduction { powerplantName = "windpark2", production = 36 },
//             new PowerplantProduction { powerplantName = "gasfiredbig1", production = 460 },
//             new PowerplantProduction { powerplantName = "gasfiredbig2", production = 264 }
//         };

//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Returns(expectedProduction);

//         // Act
//         var result = _dispatchService.DispatchProduction(request).ToList();

//         // Assert
//         Assert.Contains(result, p => p.powerplantName.Contains("wind"));
//         Assert.Contains(result, p => p.powerplantName.Contains("gasfired"));
//     }

//     [Fact]
//     public void DispatchProduction_ShouldLogInformationMessage()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             }
//         };

//         var request = new ProductionPlanRequest(910, fuels, powerplants);

//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Returns(new List<PowerplantProduction>());

//         // Act
//         _dispatchService.DispatchProduction(request);

//         // Assert
//         _loggerMock.Verify(
//             x => x.Log(
//                 It.IsAny<LogLevel>(),
//                 It.IsAny<EventId>(),
//                 It.IsAny<It.IsAnyType>(),
//                 It.IsAny<Exception>(),
//                 It.IsAny<Func<It.IsAnyType, Exception, string>>()),
//             Times.AtLeastOnce);
//     }

//     [Fact]
//     public void DispatchProduction_ShouldRespectPowerplantConstraints()
//     {
//         // Arrange
//         var fuels = new FuelsInfo(13.4, 50.8, 20, 60);
//         var powerplants = new List<PowerplantModel>
//         {
//             new PowerplantModel
//             {
//                 Name = "gasfiredbig1",
//                 Type = PowerplantType.GasFired,
//                 Efficiency = 0.53,
//                 PMin = 100,
//                 PMax = 460
//             },
//             new PowerplantModel
//             {
//                 Name = "windpark1",
//                 Type = PowerplantType.WindTurbine,
//                 Efficiency = 1,
//                 PMin = 0,
//                 PMax = 150
//             }
//         };

//         var request = new ProductionPlanRequest(480, fuels, powerplants);

//         var expectedProduction = new List<PowerplantProduction>
//         {
//             new PowerplantProduction { powerplantName = "windpark1", production = 150 },
//             new PowerplantProduction { powerplantName = "gasfiredbig1", production = 330 }
//         };

//         _productionServiceMock
//             .Setup(x => x.DispatchProduction(It.IsAny<double>(), It.IsAny<IEnumerable<PowerplantModel>>(), It.IsAny<FuelsInfo>()))
//             .Returns(expectedProduction);

//         // Act
//         var result = _dispatchService.DispatchProduction(request).ToList();

//         // Assert
//         var gasfired = result.First(p => p.powerplantName == "gasfiredbig1");
//         Assert.True(gasfired.production >= 100 || gasfired.production == 0); // Respect PMin
//         Assert.True(gasfired.production <= 460); // Respect PMax
//     }
// }