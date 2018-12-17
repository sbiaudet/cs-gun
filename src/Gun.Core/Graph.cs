using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Gun.Core
{
    public class Graph
    {
        private readonly IDictionary<string, Node> _graph = new Dictionary<string, Node>();

        public IDictionary<string,Node> Get(GetNode getNode)
        {
            var res =  new Dictionary<string, Node>();

            if (_graph.ContainsKey(getNode.Soul)){
                var node = _graph[getNode.Soul];
                if(!string.IsNullOrEmpty(getNode.Key)){
                    if(node.Properties.ContainsKey(getNode.Key)){
                        res[getNode.Soul] = node.Clone(getNode.Key);
                    }
                }
                else {
                    res[getNode.Soul] = node;
                }
            }
            
            return res;
        }

        public IDictionary<string, Node> Mix(IDictionary<string, Node> change)
        {

            var machine = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var diff = new Dictionary<string, Node>();

            foreach (var soul in change.Keys)
            {
                var node = change[soul];
                foreach(var key in node.Properties.Keys)
                {
                    var incomingValue = node.Properties[key];
                    var incomingState = node.Metadata.HAMState.ContainsKey(key) ? node.Metadata.HAMState[key] : Double.NegativeInfinity;
                    var currentNode = _graph.ContainsKey(soul) ? _graph[soul] : new Node(soul);
                    var currentState = currentNode.Metadata.HAMState.ContainsKey(key) ? currentNode.Metadata.HAMState[key] : Double.NegativeInfinity;
                    var currentValue = currentNode.Properties.ContainsKey(key) ? currentNode.Properties[key] : JToken.Parse("{}");
                    var ham = HAM.Run(machine, incomingState, currentState, incomingValue, currentValue);
                    if((ham & HAMStateResult.Incoming) != HAMStateResult.Incoming)
                    {
                        if((ham & HAMStateResult.Defer) == HAMStateResult.Defer)
                        {

                        }
                        break;
                    }
                    diff[soul] = diff.ContainsKey(soul) ? diff[soul] : new Node(soul);
                    _graph[soul] = _graph.ContainsKey(soul) ? _graph[soul] : new Node(soul);
                    _graph[soul].Properties[key] = diff[soul].Properties[key] = incomingValue;
                    _graph[soul].Metadata.HAMState[key] = diff[soul].Metadata.HAMState[key] = incomingState;

                }
            }

            return diff;
        }
    }
}