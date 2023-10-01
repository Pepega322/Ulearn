using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class DrawingInstrument
    {
        const double PI = Math.PI;
        const float LengthStraight = 0.375f;
        const float LengthDiagonal = 0.04f;

        static float x, y;
        static Graphics graphics;

        public static void Initialize(Graphics newGraphics)
        {
            graphics = newGraphics;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0;
            y = y0;
        }

        public static void MakeLine(Pen color, double length, double angle)
        {
            //Делает шаг длиной dlina в направлении ugol и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            graphics.DrawLine(color, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void ChangePosition(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        const double PI = Math.PI;
        const float LengthStraight = 0.375f;
        const float LengthDiagonal = 0.04f;

        static void MakeOneSide(double startAngle, double startSize)
        {
            DrawingInstrument.MakeLine(Pens.Yellow, startSize * LengthStraight, startAngle);
            DrawingInstrument.MakeLine(Pens.Yellow, startSize * LengthDiagonal * Math.Sqrt(2), startAngle + PI / 4);
            DrawingInstrument.MakeLine(Pens.Yellow, startSize * LengthStraight, startAngle + PI);
            DrawingInstrument.MakeLine(Pens.Yellow, startSize * LengthStraight - startSize * LengthDiagonal,
                startAngle + PI / 2);

            DrawingInstrument.ChangePosition(startSize * LengthDiagonal, startAngle - PI);
            DrawingInstrument.ChangePosition(startSize * LengthDiagonal * Math.Sqrt(2), startAngle + 3 * PI / 4);
        }

        public static void Draw(int width, int length, double rotateAngle, Graphics graphics)
        {
            // ugolPovorota пока не используется, но будет использоваться в будущем
            DrawingInstrument.Initialize(graphics);

            var size = Math.Min(width, length);

            var diagonalLength = Math.Sqrt(2) * (size * LengthStraight + size * LengthDiagonal) / 2;
            var x0 = (float)(diagonalLength * Math.Cos(PI / 4 + PI)) + width / 2f;
            var y0 = (float)(diagonalLength * Math.Sin(PI / 4 + PI)) + length / 2f;

            DrawingInstrument.SetPosition(x0, y0);
            MakeOneSide(0, size);
            MakeOneSide(-PI / 2, size);
            MakeOneSide(PI, size);
            MakeOneSide(PI / 2, size);
        }
    }
}