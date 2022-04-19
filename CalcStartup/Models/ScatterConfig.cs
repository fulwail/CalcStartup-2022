using Newtonsoft.Json;

namespace CalcStartup.Models
{
    public class ScatterConfig
    {
        [JsonProperty("x")]
        public decimal X { get; set; }

        [JsonProperty("y")]
        public decimal Y { get; set; }
    }
}
