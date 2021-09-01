using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Exceptions;
using Domain;

namespace Application.Services
{
    public class ProductionPlanService : IProductionPlanService
    {

        public List<ProductionPlanItem> Calculate(IEnumerable<PowerPlant> powerPlants, Fuel fuels, double load)
        {
            // Get the list of powerplants with cost, PowerMin and PowerMax calculated.
            var powerPlantWithCosts = powerPlants.Select(x => new PowerPlantDecorator(x, fuels))
                .ToList();

            // edge cases:

            // => load cannot be fulfilled :
            // i choose to return a message (bad request) to the caller to inform him that the load cannot be fulfilled
            // we could also raise an event to be consumed by a process for farther actions, but here i let the caller decide what to do
            var productionPlanCapacity = powerPlantWithCosts.Sum(x => x.PowerMax);
            if (productionPlanCapacity < load)
            {
                // The exception will be handled by the global error filter, a bad request will be returned
                throw new ProductionPlanException($"The expected load cannot be fulfilled, remaining load to produce:  {load - productionPlanCapacity}");
            }

            // 2 => Over production:
            // for example when the load is less than the smallest pmin
            var minimumProduction = powerPlantWithCosts.Where(x => x.CanProduce).Min(x => x.PowerMin);
            if (load < minimumProduction)
            {
                // The exception will be handled by the global error filter, a bad request will be returned
                throw new ProductionPlanException($"Over production detected, the extra production amount is: {minimumProduction - load}");
            }

            // merit-order
            var powerPlantsInMeritOrder = powerPlantWithCosts
                .OrderBy(x => x.Cost) // privilege the cheapest and the more efficient (Efficiency is taken in account)
                .ThenByDescending(x => x.PowerMax) // then the ones can produce more
                .ThenBy(x => x.PowerMin); // then the ones with minimum production when switched-on

            // the expected load
            var expectedLoad = load;
            var productionPlan = new List<ProductionPlanItem>();


            // The balance amount is the sum of the delta between the pmin and pmax of all activated powerplants.
            // This will help to balance load in case we would produce to much by switching on a plant with a
            // large pmin to achieve the required load.
            var balanceAmount = 0.0;

            foreach (var powerPlant in powerPlantsInMeritOrder)
            {

                if (!powerPlant.CanProduce || expectedLoad <= 0)
                {
                    // The load is reached and therefore the powerplant should switched off.
                    productionPlan.Add(new ProductionPlanItem
                    {
                        Name = powerPlant.Name,
                        Power = 0
                    });
                }
                else if (powerPlant.PowerMax <= expectedLoad)
                {
                    productionPlan.Add(new ProductionPlanItem
                    {
                        Name = powerPlant.Name,
                        Power = powerPlant.PowerMax,
                        PowerMin = powerPlant.PowerMin, // will be used to calculate the balance amount
                        CanBeBalanced = true
                    });
                    expectedLoad -= powerPlant.PowerMax;
                    balanceAmount = balanceAmount + powerPlant.PowerMax - powerPlant.PowerMin;

                }
                else if (powerPlant.PowerMin <= expectedLoad)
                {
                    productionPlan.Add(new ProductionPlanItem
                    {
                        Name = powerPlant.Name,
                        Power = Math.Round(expectedLoad,1)
                    });
                    expectedLoad = 0;
                } 
                else if (powerPlant.PowerMin > expectedLoad && powerPlant.PowerMin - expectedLoad <= balanceAmount)
                {
                    productionPlan.Add(new ProductionPlanItem
                    {
                        Name = powerPlant.Name,
                        Power = powerPlant.PowerMin
                    });
                  
                    BalanceProductionPlan(productionPlan.Where(x=> x.CanBeBalanced).ToList(), powerPlant.PowerMin - expectedLoad);

                    expectedLoad = 0;
                }
                else
                {
                    // Over production 
                    // The exception will be handled by the global error filter, a bad request will be returned
                    throw new ProductionPlanException($"Over production detected, the extra production amount is: {powerPlant.PowerMin - expectedLoad - balanceAmount}");
                }

            }

            return productionPlan;
        }

        private static void BalanceProductionPlan(List<ProductionPlanItem> productionPlan, double balanceAmount)
        {
            // Remove extra production from the powerplants where cost is the most expensive.
            productionPlan.Reverse();

            var toBalance = balanceAmount;

            foreach (var plan in productionPlan)
            {
                if (toBalance > 0)
                {
                    var balance = Math.Min(toBalance, plan.Power - plan.PowerMin);

                    plan.Power = Math.Round(plan.Power - balance);
                    toBalance -= balance;
                }
            }
        }
    }
}

