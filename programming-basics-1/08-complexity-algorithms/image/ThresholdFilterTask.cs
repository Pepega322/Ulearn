using System.Linq;

namespace Recognizer {
    public static class ThresholdFilterTask {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction) {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filtered = new double[width, height];
            var threshold = GetThreshold(original, whitePixelsFraction);
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++) {
                    if (original[x, y] >= threshold) filtered[x, y] = 1;
                    else filtered[x, y] = 0;
                }
            return filtered;
        }

        private static double GetThreshold(double[,] original, double whitePixelsFraction) {
            var whitePixelsCount = (int)(original.Length * whitePixelsFraction);
            if (whitePixelsCount == 0) return double.MaxValue;
            var brightness = original.Cast<double>().OrderByDescending(e => e).ToArray();
            return brightness[whitePixelsCount - 1];
        }
    }
}