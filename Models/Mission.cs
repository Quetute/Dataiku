using Newtonsoft.Json;

namespace Dataiku.Models
{
    public class Mission 
    {
        [JsonProperty( Required = Required.Always)]        
        public int Autonomy { get; set; }

        [JsonProperty( Required = Required.Always)]
        public string Departure { get; set; }

        [JsonProperty( Required = Required.Always)]
        public string Arrival { get; set; }

        [JsonProperty(PropertyName = "routes_db", Required = Required.Always)]
        public string RoutesDb { get; set; }
    }
}