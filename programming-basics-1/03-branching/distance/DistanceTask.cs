using System;

namespace DistanceTask {
    public static class DistanceTask {
        public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y) {
            var AB = new Vector(bx - ax, by - ay);
            var BA = AB.Reverse();
            var AM = new Vector(x - ax, y - ay);
            var BM = new Vector(x - bx, y - by);
            var cosBAM = AB.GetCos(AM);
            var cosABM = BA.GetCos(BM);

            if (cosABM >= 1e-9 && cosBAM >= 1e-9)
                return Math.Abs(AB.GetSkew(AM) / AB.Length);
            return Math.Min(AM.Length, BM.Length);
        }
    }

    public class Vector {
        public readonly double X;
        public readonly double Y;
        public double Length { get => Math.Sqrt(X * X + Y * Y); }

        public Vector(double x, double y) {
            X = x;
            Y = y;
        }

        public Vector Reverse() => new Vector(-X, -Y);
        public double GetScalar(Vector v) => X * v.X + Y * v.Y;
        public double GetSkew(Vector v) => X * v.Y - Y * v.X;
        public double GetCos(Vector v) => GetScalar(v) / (Length * v.Length);
        public double GetSin(Vector v) => GetSkew(v) / (Length * v.Length);
    }
}