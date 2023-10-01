using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double[,] MedianFilter(double[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filtered = new double[width, height];

            var niarPixels = new List<double>();
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    WriteNearPixels(original, width, height, x, y, niarPixels);
                    filtered[x, y] = GetMedian(niarPixels);
                    niarPixels.Clear();
                }

            return filtered;
        }

        private static void WriteNearPixels(double[,] original, int width, int height,
            int x, int y, List<double> niarPixels)
        {
            for (var i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= width) continue;
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (j < 0 || j >= height) continue;
                    niarPixels.Add(original[i, j]);
                }
            }
            niarPixels.Sort();
        }

        private static double GetMedian(List<double> niarPixels)
        {
            double median = 0;
            if (niarPixels.Count % 2 == 0)
                median = (niarPixels[niarPixels.Count / 2] + niarPixels[niarPixels.Count / 2 - 1]) / 2;
            else
                median = niarPixels[niarPixels.Count / 2];
            return median;
        }
    }
}