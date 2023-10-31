using System;

namespace Fractals {
    internal static class DragonFractalTask {
        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed) {
            var random = new Random(seed);
            double x0 = 1;
            double y0 = 0;
            double x, y;
            pixels.SetPixel(x0, y0);
            for (int i = 0; i < iterationsCount; i++) {
                var nextNumber = random.Next(2);
                if (nextNumber == 0) {
                    x = (x0 - y0) / 2;
                    y = (x0 + y0) / 2;
                }
                else {
                    x = 1 - (x0 + y0) / 2;
                    y = (x0 - y0) / 2;
                }
                pixels.SetPixel(x, y);
                x0 = x;
                y0 = y;
            }
        }
    }
}