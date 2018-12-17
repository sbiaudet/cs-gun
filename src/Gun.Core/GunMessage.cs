using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gun.Core
{
    [JsonConverter(typeof(GunMessageConverter))]
    public abstract class GunMessage
    {
        [JsonProperty("#")]
        public string Key { get; set; }

        [JsonProperty("@")]
        public string At { get; set; }
    }

    public class PutMessage : GunMessage
    {
        [JsonProperty("put")]

        public IDictionary<string, Node> PutChanges { get; set; } = new Dictionary<string, Node>();
    }

    public class GetMessage : GunMessage
    {
        [JsonProperty("get")]

        public GetNode Get { get; set; }
    }

    public class GetNode
    {
        [JsonProperty("#")]
        public string Soul { get; set; }
        [JsonProperty(".")]
        public string Key { get; set; }
    }

    public class GunMessageConverter : JsonConverter<GunMessage>
    {

        public override bool CanWrite => false;

        public override GunMessage ReadJson(JsonReader reader, Type objectType, GunMessage existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        private GunMessage Create(Type objectType, JObject jsonObject)
        {
            if (jsonObject.ContainsKey("put"))
                return new PutMessage();

            if (jsonObject.ContainsKey("get"))
                return new GetMessage();

            throw new JsonException("Unknown message to GunMessage");
        }

        public override void WriteJson(JsonWriter writer, GunMessage value, JsonSerializer serializer)
        {
            throw new NotSupportedException("GunMessageConverter should only be used while deserializing.");
        }
    }
}
