using Newtonsoft.Json;

namespace Domain
{
    public class ProductionPlanItem
    {
        public string Name { get; set; }
        [JsonProperty("p")]
        public double Power { get; set; }
        [JsonIgnore]
        public double PowerMin { get; set; }

        [JsonIgnore] 
        public bool CanBeBalanced { get; set; } = false;
    }
}
