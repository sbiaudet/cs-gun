using System;
using System.Collections.Generic;
using System.Text;

namespace Gun.Core.Extensions
{
    public static class RandomExtensions
    {
        public static long NextLong(this Random random) => NextLong(random, long.MaxValue);

        public static long NextLong(this Random random, long maxValue) => NextLong(random, long.MinValue, maxValue);
        

        public static long NextLong(this Random random, long minValue, long maxValue)
        {
            ulong range = (ulong)(maxValue - minValue);

            ulong longRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                longRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (longRand > ulong.MaxValue - (((ulong.MaxValue % range) + 1) % range));

            return (long)(longRand % range) + minValue;
        }
    }
}
