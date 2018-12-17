using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Gun.Core
{
    public class HAM
    {
        

        public static HAMStateResult Run(long machineState, double incomingState, double currentState, JToken incomingValue, JToken currentValue)
        {
            HAMStateResult res = 0L;

            if (machineState < incomingState)
            {
                res = res | HAMStateResult.Defer;    
            }
            if (incomingState < currentState)
            {
                res = res | HAMStateResult.Historical;
            }
            if (currentState < incomingState)
            {
                res = res | HAMStateResult.Converge | HAMStateResult.Incoming; 
            }
            if (incomingState == currentState)
            {
                var incomingStr = incomingValue.ToString();
                var currentStr = currentValue.ToString();

                if (incomingStr == currentStr)
                {
                    res = res | HAMStateResult.State;
                }

                if (string.Compare(incomingStr , currentStr) < 0)
                {
                    res = res | HAMStateResult.Converge | HAMStateResult.Current;
                }

                if (string.Compare(currentStr, incomingStr) < 0)
                {
                    res = res | HAMStateResult.Converge | HAMStateResult.Incoming;
                }
            }

            return res;

        }
    }

     public enum HAMStateResult
        {
            Defer = 0x1,
            Historical = 0x10,
            Converge = 0x100,
            Incoming = 0x1000,
            State = 0x10000,
            Current = 0x100000
        }
}
