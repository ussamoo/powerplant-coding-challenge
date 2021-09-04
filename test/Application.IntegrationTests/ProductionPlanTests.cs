using Domain;
using Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.ProductionPlan.Commands;

namespace Application.IntegrationTests
{
    using static Testing;
    public class ProductionPlanTests
    {

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new ProductionPlanCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldReturnsTheProductionPlanWithTheExpectedLoadForSample1()
        {
            // Arrange
            var command = new ProductionPlanCommand()
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
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            var resultingLoad = result.Sum(x => x.Power);
            resultingLoad.Should().Be(command.Load);
        }



        [Test]
        public async Task ShouldReturnsTheProductionPlanWithTheExpectedLoadForSample2()
        {
            // Arrange
            var command = new ProductionPlanCommand()
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
            };
            // Act
            var result = await SendAsync(command);

            // Assert
            var resultingLoad = result.Sum(x => x.Power);
            resultingLoad.Should().Be(command.Load);
        }
    }
}
