using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gun.Core
{
    public class GunMessage
    {
        [JsonProperty("#")]
        public string Key { get; set; }
        
        [JsonProperty("@")]
        public string At { get; set; }
        
        [JsonProperty("put")]

        public IDictionary<string, Node> PutChanges { get; set; } = new Dictionary<string, Node>();

        [JsonProperty("get")]

        public GetNode Get { get; set; }

    }

    public class GetNode {
        [JsonProperty("#")]
        public string Soul { get; set; }
        [JsonProperty(".")]
        public string Key { get; set; }
        

    }
}
