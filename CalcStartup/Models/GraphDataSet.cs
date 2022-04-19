using Newtonsoft.Json;

namespace CalcStartup.Models
{
    public class GraphDataSet
    {
        [JsonProperty("label")]
        public string LabelForDataSet { get; set; }

        [JsonProperty("data")]
        public List<ScatterConfig> DataForDataSet { get; set; }

        [JsonProperty("fill")]
        public bool IsFilled { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColorForDataSet { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColorForDataSet { get; set; }

        [JsonProperty("showLine")]
        private bool IsLineShowed
        {
            get { return true; }
        }

        [JsonProperty("pointRadius")]
        private double PointRadiusForDataSet
        {
            get { return 0; }
        }

        [JsonProperty("borderWidth")]
        private double BorderWidthForDataSet
        {
            get { return 2; }
        }
    }

}
