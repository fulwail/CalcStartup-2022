using CalcStartup.Models;
using Newtonsoft.Json;

namespace CalcStartup.ViewModels
{
    public class ChartViewModelList
    {
        [JsonProperty("datasets")]
        public List<GraphDataSet> DataSets { get; set; }
    }
}
