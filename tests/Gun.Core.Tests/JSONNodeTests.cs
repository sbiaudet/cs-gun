using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Gun.Core.Tests
{
    public class JSONNodeTests
    {

        [Fact]
        public void ShouldDeserializeBasic()
        {
            var jsonText = "{ age: 23, hacker: true, name: \"Mark Nadal\"}";

            var res = JsonConvert.DeserializeObject<Node>(jsonText);
            Assert.Null(res.Metadata);
            Assert.Equal(3, res.Properties.Count);
            Assert.Equal(23, res.Properties["age"].ToObject<int>());
        }

        [Fact]
        public void ShouldDeserializeMetaData()
        {
            var jsonText = "{_:{}, age: 23, hacker: true, name: \"Mark Nadal\"}";

            var res = JsonConvert.DeserializeObject<Node>(jsonText);
            Assert.NotNull(res.Metadata);
            Assert.Equal(3, res.Properties.Count);
            Assert.Equal(23, res.Properties["age"].ToObject<int>());
        }

        [Fact]
        public void ShouldDeserializeMetaDataWithSoul ()
        {
            var jsonText = "{_: {'#':'ASDF'}, age: 23, hacker: true, name: \"Mark Nadal\"}";

            var res = JsonConvert.DeserializeObject<Node>(jsonText);
            Assert.NotNull(res.Metadata);
            Assert.Equal("ASDF", res.Metadata.Soul);
            Assert.Equal(3, res.Properties.Count);
            Assert.Equal(23, res.Properties["age"].ToObject<int>());
        }

        [Fact]
        public void ShouldDeserializeMetaDataWithHAMState()
        {
            var jsonText = "{_: {'#':'ASDF', '>': {age:2, hacker:2, name:2}}, age: 23, hacker: true, name: \"Mark Nadal\"}";

            var res = JsonConvert.DeserializeObject<Node>(jsonText);
            Assert.NotNull(res.Metadata);
            Assert.Equal("ASDF", res.Metadata.Soul);
            Assert.NotNull(res.Metadata.HAMState);
            Assert.Equal(2, res.Metadata.HAMState["age"]);
            Assert.Equal(3, res.Properties.Count);
            Assert.Equal(23, res.Properties["age"].ToObject<int>());
        }
    }
}
