using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Gun.Core
{
    public class HAM
    {
        public static Dictionary<string, Node> Mix(IDictionary<string, Node> change, IDictionary<string, Node> graph)
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
                    var currentNode = graph.ContainsKey(soul) ? graph[soul] : new Node(soul);
                    var currentState = currentNode.Metadata.HAMState.ContainsKey(key) ? currentNode.Metadata.HAMState[key] : Double.NegativeInfinity;
                    var currentValue = currentNode.Properties.ContainsKey(key) ? currentNode.Properties[key] : JToken.Parse("{}");
                    var ham = HAM.Run(machine, incomingState, currentState, incomingValue, currentValue);
                    if((ham & HAMState.Incoming) != HAMState.Incoming)
                    {
                        if((ham & HAMState.Defer) == HAMState.Defer)
                        {

                        }
                        break;
                    }
                    diff[soul] = diff.ContainsKey(soul) ? diff[soul] : new Node(soul);
                    graph[soul] = graph.ContainsKey(soul) ? graph[soul] : new Node(soul);
                    graph[soul].Properties[key] = diff[soul].Properties[key] = incomingValue;
                    graph[soul].Metadata.HAMState[key] = diff[soul].Metadata.HAMState[key] = incomingState;

                }
            }

            return diff;
        }

        public static HAMState Run(long machineState, double incomingState, double currentState, JToken incomingValue, JToken currentValue)
        {
            HAMState res = 0L;

            if (machineState < incomingState)
            {
                res = res | HAMState.Defer;    
            }
            if (incomingState < currentState)
            {
                res = res | HAMState.Historical;
            }
            if (currentState < incomingState)
            {
                res = res | HAMState.Converge | HAMState.Incoming; 
            }
            if (incomingState == currentState)
            {
                var incomingStr = incomingValue.ToString();
                var currentStr = currentValue.ToString();

                if (incomingStr == currentStr)
                {
                    res = res | HAMState.State;
                }

                if (string.Compare(incomingStr , currentStr) < 0)
                {
                    res = res | HAMState.Converge | HAMState.Current;
                }

                if (string.Compare(currentStr, incomingStr) < 0)
                {
                    res = res | HAMState.Converge | HAMState.Incoming;
                }
            }

            return res;

        }

        public enum HAMState
        {
            Defer = 0x1,
            Historical = 0x10,
            Converge = 0x100,
            Incoming = 0x1000,
            State = 0x10000,
            Current = 0x100000
        }
    }
}
