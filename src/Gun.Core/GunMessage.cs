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

        [JsonProperty("put")]

        public IDictionary<string, Node> PutChanges { get; set; } = new Dictionary<string, Node>();

        [JsonProperty("get")]

        public IDictionary<string, Node> GetChanges { get; set; } = new Dictionary<string, Node>();

    }
}
