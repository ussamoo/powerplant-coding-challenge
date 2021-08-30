using Domain.Enums;
using System;

namespace Domain
{
    public class PowerPlantDecorator
    {

        // default constructor
        private PowerPlantDecorator()
        {
            _fuels = new Fuel();
            _powerPlant = new PowerPlant();
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="PowerPlantDecorator"/> object
        /// </summary>
        /// <param name="basePowerPlant"><see cref="PowerPlant"/></param>
        /// <param name="fuels"><see cref="Fuel"/></param>
        public PowerPlantDecorator(PowerPlant basePowerPlant, Fuel fuels)
        {
            _fuels = fuels ?? throw new ArgumentNullException(nameof(fuels));

            _powerPlant = basePowerPlant ?? throw new ArgumentNullException(nameof(basePowerPlant));
        }

        private readonly PowerPlant _powerPlant;

        private readonly Fuel _fuels;

        /// <summary>
        /// Gets the name of the powerplant.
        /// </summary>
        public string Name => _powerPlant.Name;

        public PowerPlantType Type => _powerPlant.Type;

        /// <summary>
        /// Gets the efficiency of the the powerplant.
        /// </summary>
        public double Efficiency => _powerPlant.Efficiency;

        /// <summary>
        /// Gets the minimum amount of power the powerplant generates when switched on.
        /// </summary>
        public double PowerMin => Type == PowerPlantType.WindTurbine ? Math.Round(_powerPlant.PMax * _fuels.Wind / 100,1) : _powerPlant.PMin;
        /// <summary>
        /// Gets the maximum amount of power the powerplant can generate.
        /// </summary>
        public double PowerMax => Type == PowerPlantType.WindTurbine ? Math.Round(_powerPlant.PMax * _fuels.Wind / 100,1) : _powerPlant.PMax;

        /// <summary>
        /// Gets the cost.
        /// </summary>
        public double Cost => GetCost();
        /// <summary>
        /// A flag indicating if the power plant can produce power (especially helpful for wind turbine)
        /// </summary>
        public bool CanProduce => PowerMax > 0;
        private double GetCost()
        {
            return Type switch
            {
                PowerPlantType.WindTurbine => 0,
                PowerPlantType.GasFired => Math.Round(_fuels.Gas / Efficiency,1), //TODO take in account the CO2 cost
                _ => Math.Round(_fuels.Kerosine / Efficiency,1)
            };
        }

    }
}
