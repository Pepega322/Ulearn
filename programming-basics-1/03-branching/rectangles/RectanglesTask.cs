using System;
using System.Drawing;
using System.Globalization;

namespace Rectangles
{
    public static class RectanglesTask
    {
        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            bool isIntersectionByX = !((r1.Right < r2.Left) || (r2.Right < r1.Left));
            bool isIntersectionByY = !((r1.Bottom < r2.Top) || (r2.Bottom < r1.Top));
            return isIntersectionByX && isIntersectionByY;
        }

        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (!AreIntersected(r1, r2)) return 0;
            var width = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left);
            var height = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top);
            return width * height;
        }

        public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            bool is1Nested = (r2.Left <= r1.Left && r1.Right <= r2.Right) 
                && (r2.Top <= r1.Top && r1.Bottom <= r2.Bottom);
            bool is2Nested = (r1.Left <= r2.Left && r2.Right <= r1.Right) 
                && (r1.Top <= r2.Top && r2.Bottom <= r1.Bottom);
            if (is1Nested) return 0;
            else if (is2Nested) return 1;
            else return -1;
        }
    }
}