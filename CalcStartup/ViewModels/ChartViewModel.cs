using Newtonsoft.Json;

namespace CalcStartup.ViewModels
{
    public class 
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("firstValue")]
        public decimal FirstValue { get; set; }
        [JsonProperty("endValue")]
        public decimal EndValue { get; set; }
    }
}
