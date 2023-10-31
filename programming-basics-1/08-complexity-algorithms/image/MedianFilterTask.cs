using System.Collections.Generic;

namespace Recognizer {
    internal static class MedianFilterTask {
        public static double[,] MedianFilter(double[,] original) {
            var width = original.GetLength(0);
            var height = original.GetLength(1);

            var filtered = new double[width, height];
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    filtered[x, y] = GetMedian(original, x, y);
            return filtered;
        }

        private static double GetMedian(double[,] original, int x, int y) {
            var width = original.GetLength(0);
            var height = original.GetLength(1);

            var nearPixels = new List<double>();
            for (var i = x - 1; i <= x + 1; i++)
                for (var j = y - 1; j <= y + 1; j++) {
                    if ((i < 0 || i >= width) || (j < 0 || j >= height)) continue;
                    nearPixels.Add(original[i, j]);
                }
            nearPixels.Sort();

            if (nearPixels.Count % 2 != 0) return nearPixels[nearPixels.Count / 2];
            return (nearPixels[nearPixels.Count / 2] + nearPixels[nearPixels.Count / 2 - 1]) / 2;
        }
    }
}