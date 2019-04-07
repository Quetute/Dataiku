using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Dataiku.Models
{
    public class BountyHunter
    {
        [JsonProperty(Required = Required.Always)]
        public string Planet { get; set; }

        [Range(0, int.MaxValue)]
        public int Day { get; set; }
    }
}