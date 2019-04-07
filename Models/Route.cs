using Newtonsoft.Json;

namespace Dataiku.Models
{
    public class Route 
    {
        [JsonProperty(Required = Required.Always)]
        public string Origin { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Destination { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int TravelTime { get; set; }
    }
}