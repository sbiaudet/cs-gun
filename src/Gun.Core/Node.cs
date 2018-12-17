using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gun.Core
{
    public class Node
    {
        public Node()
        {

        }

        public Node(string soul)
        {
            this.Metadata = new Metadata() { Soul = soul };
        }

        [JsonProperty("_")]
        public Metadata Metadata { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Properties { get; set; } = new Dictionary<string, JToken>();

    }

    public class Metadata
    {
        [JsonProperty("#")]

        public string Soul { get; set; }

        [JsonProperty(">")]

        public HAMState HAMState { get; set; } = new HAMState();
    }

    [JsonDictionary()]
    public class HAMState : Dictionary<string, double>
    {

    }
}
