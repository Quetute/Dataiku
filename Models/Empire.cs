using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dataiku.Models
{
    public class Empire {

        [JsonProperty(Required=Required.Always)]
        public int Countdown { get; set; }

        [JsonProperty(PropertyName="bounty_hunters")]
        public List<BountyHunter> BountyHunters { get; set; }
    }
}