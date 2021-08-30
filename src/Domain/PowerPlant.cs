using Domain.Enums;
using Newtonsoft.Json;

namespace Domain
{
    public class PowerPlant
    {
        public string Name { get; set; }
        public PowerPlantType Type { get; set; }

        public double Efficiency { get; set; }

        [JsonProperty("pmin")]
        public double PMin { get; set; }

        [JsonProperty("pmax")]
        public double PMax { get; set; }

    }
}
