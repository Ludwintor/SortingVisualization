using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        /// <summary>
        /// Transforms HSL to RGB <see cref="Color"/>
        /// </summary>
        /// <remarks>
        /// <see href="https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_RGB">Transform algorithm on wikipedia</see>
        /// </remarks>
        /// <param name="hue">Must be in range [0; 360)</param>
        /// <param name="saturation">Must be in range [0; 1]</param>
        /// <param name="lightness">Must be in range [0; 1]</param>
        /// <returns>RGB <see cref="Color"/></returns>
        public static Color HSL2RGB(double hue, double saturation, double lightness)
        {
            hue = Math.Clamp(hue, 0d, 359d);
            saturation = Math.Clamp(saturation, 0d, 1d);
            lightness = Math.Clamp(lightness, 0d, 1d);
            double chroma = (1d - Math.Abs(2d * lightness - 1d)) * saturation;
            double partitionHue = hue / 60d;
            double secondChroma = chroma * (1d - Math.Abs(partitionHue % 2d - 1d));
            Color rgb = RGBFromChroma(chroma, secondChroma, (int)partitionHue);
            int lightnessMatch = (int)Math.Round((lightness - chroma / 2d) * 255d);
            return Color.FromArgb(rgb.R + lightnessMatch,
                                  rgb.G + lightnessMatch,
                                  rgb.B + lightnessMatch);
        }

        /// <summary>
        /// Determine raw RGB color (no lightness affected)
        /// </summary>
        /// <param name="chroma">First largest component</param>
        /// <param name="secondChroma">Second largest component</param>
        /// <param name="hueIndex">Hue index in range [0; 5]</param>
        /// <returns>RGB color without lightness</returns>
        /// <exception cref="ArgumentOutOfRangeException">If hue index outside [0; 5]</exception>
        private static Color RGBFromChroma(double chroma, double secondChroma, int hueIndex)
        {
            int r = (int)(chroma * 255d);
            int g = (int)(secondChroma * 255d);
            return hueIndex switch
            {
                0 => Color.FromArgb(r, g, 0),
                1 => Color.FromArgb(g, r, 0),
                2 => Color.FromArgb(0, r, g),
                3 => Color.FromArgb(0, g, r),
                4 => Color.FromArgb(g, 0, r),
                5 => Color.FromArgb(r, 0, g),
                _ => throw new ArgumentOutOfRangeException(nameof(hueIndex))
            };
        }
    }
}
