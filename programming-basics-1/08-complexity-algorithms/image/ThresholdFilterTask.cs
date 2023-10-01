using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filtered = new double[width, height];

            var thresholdValue = GetThresholdValue(original, whitePixelsFraction);
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    if (original[x, y] >= thresholdValue) filtered[x, y] = 1;
                    else filtered[x, y] = 0;
                }

            return filtered;
        }

        private static double GetThresholdValue(double[,] original, double whitePixelsFraction)
        {
            var whitePixelsCount = (int)(original.Length * whitePixelsFraction);
            if (whitePixelsCount == 0) return double.MaxValue;
            var brightnessValues = GetBrightnessValues(original);

            return brightnessValues[whitePixelsCount - 1];
        }

        private static List<double> GetBrightnessValues(double[,] original)
        {
            var brightnessValues = new List<double>();
            foreach (var value in original)
                brightnessValues.Add(value);
            brightnessValues.Sort();
            brightnessValues.Reverse();

            return brightnessValues;
        }
    }
}