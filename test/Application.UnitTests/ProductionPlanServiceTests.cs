using System.Collections.Generic;
using System.Linq;
using Application.Common.Exceptions;
using Application.ProductionPlan.Commands;
using Application.Services;
using Domain;
using Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests
{
    public class ProductionPlanServiceTests
    {
        [Test]
        public void ShouldReturnsTheProductionPlanWithMostEfficientGenerateMorePower()
        {
            // Arrange
            var load = 240;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 60, Wind = 20 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 60, PMax = 200 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 60, PMax = 200 }
            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(240);
            var first = result.First();
            var last = result.Last();
            first.Name.Should().Be("gas2");
            first.Power.Should().BeGreaterThan(last.Power);
        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithOneGasFireOff()
        {
            // Arrange
            var load = 230;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 10, PMax = 100 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 200 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind2", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 }

            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(230);
            result.FirstOrDefault(x => x.Name == "gas1")?.Power.Should().Be(0);
        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithOneGasFireAtMinimumProduction()
        {
            // Arrange
            var load = 232;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 10, PMax = 100 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 200 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind2", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 }

            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(232);
            result.FirstOrDefault(x => x.Name == "gas1")?.Power.Should().Be(10);
        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithTurbojetOff()
        {
            // Arrange
            var load = 300;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 10, PMax = 100 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 200 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind2", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 48 },
                new PowerPlant { Name = "tj1", Type = PowerPlantType.Turbojet, Efficiency = 0.8, PMin = 0, PMax = 16 }


            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(300);
            result.FirstOrDefault(x => x.Name == "tj1")?.Power.Should().Be(0);
        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithTurbojetAtMaximumProduction()
        {
            // Arrange
            var load = 300;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 12.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 10, PMax = 100 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 200 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind2", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 48 },
                new PowerPlant { Name = "tj1", Type = PowerPlantType.Turbojet, Efficiency = 0.8, PMin = 0, PMax = 16 }


            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(300);
            result.FirstOrDefault(x => x.Name == "tj1")?.Power.Should().Be(16);
        }

        [Test]
        public void ShouldReturnsTheProductionPlanWithTurbojetAtMaximumProductionAndOneGasFireOff()
        {
            // Arrange
            var load = 255;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 12.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 10, PMax = 100 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 200 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind2", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 48 },
                new PowerPlant { Name = "tj1", Type = PowerPlantType.Turbojet, Efficiency = 0.8, PMin = 0, PMax = 16 }
            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(255);
            result.FirstOrDefault(x => x.Name == "tj1")?.Power.Should().Be(16);
            result.FirstOrDefault(x => x.Name == "gas1")?.Power.Should().Be(0);

        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithEfficiencyTakenInAccount()
        {
            // Arrange
            var load = 350;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 80 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "tj1", Type = PowerPlantType.Turbojet, Efficiency = 0.3, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 100 },
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.5, PMin = 100, PMax = 300 }

            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(350);
            var first = result.First();
            first.Name.Should().Be("wind1");
            first.Power.Should().Be(80);
            result.FirstOrDefault(x => x.Name == "tj1")?.Power.Should().Be(0);

        }



        [Test]
        public void ShouldReturnsTheProductionPlanWithMostEfficientOff()
        {
            // Arrange
            var load = 300;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 60 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 120 },
                new PowerPlant { Name = "gas2", Type = PowerPlantType.GasFired, Efficiency = 0.75, PMin = 10, PMax = 50 },
                new PowerPlant { Name = "gas3", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 120 },
                new PowerPlant { Name = "gas4", Type = PowerPlantType.GasFired, Efficiency = 0.53, PMin = 100, PMax = 120 }

            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(300);
            result.FirstOrDefault(x => x.Name == "gas2")?.Power.Should().Be(0);

        }


        [Test]
        public void ShouldReturnsTheProductionPlanWithTheExpectedLoadForDecimalLoad()
        {
            // Arrange
            var load = 350.5;
            var fuels = new Fuel { Gas = 13.4, Kerosine = 50.8, Co2 = 20, Wind = 80 };
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Name = "tj1", Type = PowerPlantType.Turbojet, Efficiency = 0.3, PMin = 0, PMax = 25 },
                new PowerPlant { Name = "wind1", Type = PowerPlantType.WindTurbine, Efficiency = 1, PMin = 0, PMax = 100 },
                new PowerPlant { Name = "gas1", Type = PowerPlantType.GasFired, Efficiency = 0.5, PMin = 100, PMax = 300 }

            };
            // Act
            var productionPlanService = new ProductionPlanService();
            var result = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            result.Sum(x => x.Power).Should().Be(350.5);
            var first = result.First();
            first.Name.Should().Be("wind1");
            first.Power.Should().Be(80);
            result.FirstOrDefault(x => x.Name == "tj1")?.Power.Should().Be(0);
            result.FirstOrDefault(x => x.Name == "gas1")?.Power.Should().Be(270.5);
        }


        [Test]
        public void ShouldThrowExceptionWhenLoadCannotBeFulfilled()
        {
            // Arrange.
            var load = 600;
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant { Efficiency = 1, Name = "GasFired1", PMin = 10, PMax = 200, Type = PowerPlantType.GasFired},
                new PowerPlant { Efficiency = 1, Name = "GasFired2", PMin = 100, PMax = 300, Type = PowerPlantType.GasFired}
            };
            var fuels = new Fuel { Co2 = 1, Gas = 2, Kerosine = 3,};

            // Act.
            // Assert.
            var productionPlanService = new ProductionPlanService();
            FluentActions.Invoking(() => productionPlanService.Calculate(powerPlants, fuels, load)).Should().Throw<ProductionPlanException>();
        }

        [Test]
        [TestCaseSource(nameof(_overProductionCommands))]
        public void ShouldThrowExceptionWhenOverProductionIsDetected(ProductionPlanCommand command)
        {
            // Arrange.
            var load = command.Load;
            var powerPlants = command.PowerPlants;
            var fuels = command.Fuels;

            // Act.
            // Assert.
            var productionPlanService = new ProductionPlanService();
            FluentActions.Invoking(() => productionPlanService.Calculate(powerPlants.ToList(), fuels, load)).Should().Throw<ProductionPlanException>();
        }

        [Test]
        [TestCaseSource(nameof(_commands))]
        public void ShouldReturnsTheProductionPlanWithTheExpectedLoadForThe3Samples(ProductionPlanCommand command)
        {
            // Arrange
            var load = command.Load;
            var fuels = command.Fuels;
            var powerPlants = command.PowerPlants;
            // Act
            var productionPlanService = new ProductionPlanService();
            var productionPlan = productionPlanService.Calculate(powerPlants.ToList(), fuels, load);
            // Assert
            productionPlan.Sum(x => x.Power).Should().Be(load);
        }

        private static readonly ProductionPlanCommand[] _overProductionCommands =
        {
            new ProductionPlanCommand
            {
                Load = 55,
                Fuels = new Fuel
                {
                    Gas = 13.4,
                    Kerosine = 50.8,
                    Co2 = 20,
                    Wind = 0
                },
                PowerPlants = new List<PowerPlant>
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 90,
                        PMax = 100
                    }
                }
            },
            new ProductionPlanCommand
            {
                Load = 130,
                Fuels = new Fuel
                {
                    Gas = 13.4,
                    Kerosine = 50.8,
                    Co2 = 20,
                    Wind = 0
                },
                PowerPlants = new List<PowerPlant>
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 90,
                        PMax = 100
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredbig2",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 80,
                        PMax = 100
                    }
                }
            }

        };

        private static readonly ProductionPlanCommand[] _commands =
        {
            new ProductionPlanCommand
            {
                Load = 480,
                Fuels = new Fuel
                {
                    Gas = 13.4,
                    Kerosine = 50.8,
                    Co2 = 20,
                    Wind = 60
                },
                PowerPlants = new List<PowerPlant>
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredbig2",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredsomewhatsmaller",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.37,
                        PMin = 40,
                        PMax = 210
                    },
                    new PowerPlant
                    {
                        Name = "tj1",
                        Type = PowerPlantType.Turbojet,
                        Efficiency = 0.3,
                        PMin = 0,
                        PMax = 16
                    },
                    new PowerPlant
                    {
                        Name = "windpark1",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 150
                    },
                    new PowerPlant
                    {
                        Name = "windpark2",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 36
                    }
                }
            },
            new ProductionPlanCommand
            {
                Load = 910,
                Fuels = new Fuel
                {
                    Gas = 13.4,
                    Kerosine = 50.8,
                    Co2 = 20,
                    Wind = 60
                },
                PowerPlants = new List<PowerPlant>
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredbig2",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredsomewhatsmaller",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.37,
                        PMin = 40,
                        PMax = 210
                    },
                    new PowerPlant
                    {
                        Name = "tj1",
                        Type = PowerPlantType.Turbojet,
                        Efficiency = 0.3,
                        PMin = 0,
                        PMax = 16
                    },
                    new PowerPlant
                    {
                        Name = "windpark1",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 150
                    },
                    new PowerPlant
                    {
                        Name = "windpark2",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 36
                    }
                }
            },
            new ProductionPlanCommand
            {
                Load = 910,
                Fuels = new Fuel
                {
                    Gas = 13.4,
                    Kerosine = 50.8,
                    Co2 = 20,
                    Wind = 0
                },
                PowerPlants = new List<PowerPlant>
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredbig2",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.53,
                        PMin = 100,
                        PMax = 460
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredsomewhatsmaller",
                        Type = PowerPlantType.GasFired,
                        Efficiency = 0.37,
                        PMin = 40,
                        PMax = 210
                    },
                    new PowerPlant
                    {
                        Name = "tj1",
                        Type = PowerPlantType.Turbojet,
                        Efficiency = 0.3,
                        PMin = 0,
                        PMax = 16
                    },
                    new PowerPlant
                    {
                        Name = "windpark1",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 150
                    },
                    new PowerPlant
                    {
                        Name = "windpark2",
                        Type = PowerPlantType.WindTurbine,
                        Efficiency = 1,
                        PMin = 0,
                        PMax = 36
                    }
                }
            },
    };
    }
}