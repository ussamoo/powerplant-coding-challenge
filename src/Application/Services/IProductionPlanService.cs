using Domain;
using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// Production plan calculation interface.
    /// </summary>
    public interface IProductionPlanService
    {
        /// <summary>
        /// Calculate how much power each of a multitude of different powerplants need to produce ( production plan).
        /// </summary>
        /// <param name="powerPlants"><see cref="PowerPlant"/></param>
        /// <param name="fuels"><see cref="Fuel"/></param>
        /// <param name="load">Expected load</param>
        /// <returns>A list of <see cref="ProductionPlan"/></returns>
        List<Domain.ProductionPlanItem> Calculate(List<PowerPlant> powerPlants, Fuel fuels, double load);

    }
}
