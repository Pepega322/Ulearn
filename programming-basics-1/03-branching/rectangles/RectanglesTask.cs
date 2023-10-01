using System;

namespace Rectangles {
    public static class RectanglesTask {
        public static bool AreIntersected(Rectangle r0, Rectangle r1) {
            var isIntersectedByX = !((r0.Right < r1.Left) || (r1.Right < r0.Left));
            var isIntersectedByY = !((r0.Bottom < r1.Top) || (r1.Bottom < r0.Top));
            return isIntersectedByX && isIntersectedByY;
        }

        public static int IntersectionSquare(Rectangle r0, Rectangle r1) {
            if (!AreIntersected(r0, r1)) return 0;
            var width = Math.Min(r0.Right, r1.Right) - Math.Max(r0.Left, r1.Left);
            var height = Math.Min(r0.Bottom, r1.Bottom) - Math.Max(r0.Top, r1.Top);
            return width * height;
        }

        public static int IndexOfInnerRectangle(Rectangle r0, Rectangle r1) {
            if (r0.IsInnerIn(r1)) return 0;
            if (r1.IsInnerIn(r0)) return 1;
            return -1;
        }
    }

    public static class RectangleExtensions {
        public static bool IsInnerIn(this Rectangle rect, Rectangle innerIn) =>
            (innerIn.Left <= rect.Left && rect.Right <= innerIn.Right)
                && (innerIn.Top <= rect.Top && rect.Bottom <= innerIn.Bottom);
    }
}