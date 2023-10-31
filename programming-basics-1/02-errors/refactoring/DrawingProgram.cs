using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe {
    public static class DrawingInstrument {
        private static float x, y;
        private static Graphics graphics;

        public static void Initialize(Graphics g) {
            graphics = g;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0) {
            x = x0;
            y = y0;
        }

        public static void ChangePosition(double length, double angle) {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }

        public static void MakeLine(Pen color, double length, double angle) {
            var x1 = x;
            var y1 = y;
            ChangePosition(length, angle);
            graphics.DrawLine(color, x1, y1, x, y);
        }
    }

    public class ImpossibleSquare {
        const double PI = Math.PI;
        const float StraightLine = 0.375f;
        const float DiagonalLine = 0.04f;

        private static void DrawSide(double angle, double size) {
            var pen = Pens.Yellow;
            DrawingInstrument.MakeLine(pen, size * StraightLine, angle);
            DrawingInstrument.MakeLine(pen, size * DiagonalLine * Math.Sqrt(2), angle + PI / 4);
            DrawingInstrument.MakeLine(pen, size * StraightLine, angle + PI);
            DrawingInstrument.MakeLine(pen, size * StraightLine - size * DiagonalLine, angle + PI / 2);
            DrawingInstrument.ChangePosition(size * DiagonalLine, angle - PI);
            DrawingInstrument.ChangePosition(size * DiagonalLine * Math.Sqrt(2), angle + 3 * PI / 4);
        }

        private static void SetInstrumentOnStart(int width, int length, int size) {
            var diagonalLength = Math.Sqrt(2) * (size * StraightLine + size * DiagonalLine) / 2;
            var x0 = (float)(diagonalLength * Math.Cos(PI / 4 + PI)) + width / 2f;
            var y0 = (float)(diagonalLength * Math.Sin(PI / 4 + PI)) + length / 2f;
            DrawingInstrument.SetPosition(x0, y0);
        }

        public static void Draw(int width, int length, double rotateAngle, Graphics graphics) {
            DrawingInstrument.Initialize(graphics);
            var size = Math.Min(width, length);
            SetInstrumentOnStart(width, length, size);
            DrawSide(0, size);
            DrawSide(-PI / 2, size);
            DrawSide(PI, size);
            DrawSide(PI / 2, size);
        }
    }
}