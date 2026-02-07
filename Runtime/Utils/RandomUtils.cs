using System;
using Random = UnityEngine.Random;

namespace IRG
{
    public static class RandomUtils
    {
        public static long GetLong()
        {
            return (DateTime.UtcNow.Ticks << 16) | (ushort)Random.Range(0, ushort.MaxValue);
        }
    }
}