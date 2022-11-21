using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingVisualization
{
    public static class Utils
    {
        public static void BadShuffle<T>(T[] array, Random rng, Action<int>? onStep)
        {
            for (int i = 0; i < array.Length; i++)
            {
                onStep?.Invoke(i);
                int j = rng.Next(array.Length);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
