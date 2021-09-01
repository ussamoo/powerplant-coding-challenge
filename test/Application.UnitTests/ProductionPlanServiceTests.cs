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
            FluentActions.Invoking(() => productionPlanService.Calculate(powerPlants, fuels, load)).Should().Throw<ProductionPlanException>();
        }

        [Test]
        [TestCaseSource(nameof(_commands))]
        public void ShouldReturnsTheProductionPlanWithTheExpectedLoad(ProductionPlanCommand command)
        {
            // Arrange
            var load = command.Load;
            var fuels = command.Fuels;
            var powerPlants = command.PowerPlants;
            // Act
            var productionPlanService = new ProductionPlanService();
            var productionPlan = productionPlanService.Calculate(powerPlants, fuels, load);
            // Assert
            productionPlan.Sum(x => x.Power).Should().Be(load);
        }

        private static ProductionPlanCommand[] _overProductionCommands =
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

        private static ProductionPlanCommand[] _commands =
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