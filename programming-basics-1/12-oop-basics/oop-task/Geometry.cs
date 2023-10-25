using System;

namespace GeometryTasks {
    public static class Geometry {
        public static double GetLength(this Vector v)
            => Math.Sqrt(v.X * v.X + v.Y * v.Y);

        public static double GetSkew(Vector v1, Vector v2)
            => v1.X * v2.Y - v2.X - v1.Y;

        public static double GetScalar(Vector v1, Vector v2)
            => v1.X * v2.X + v1.Y * v2.Y;

        public static double GetAngle(Vector v1, Vector v2) {
            var l1 = GetLength(v1);
            var l2 = GetLength(v2);
            if (l1 < 1e-6 || l2 < 1e-6) throw new Exception("Zero vector");
            return Math.Acos(GetScalar(v1, v2) / (l1 * l2));
        }

        public static Vector Add(Vector v1, Vector v2)
            => new Vector(v1.X + v2.X, v1.Y + v2.Y);

        public static double GetLength(this Segment s)
            => GetLength(new Vector(s.End.X - s.Begin.X, s.End.Y - s.Begin.Y));

        public static bool IsVectorInSegment(Vector p, Segment s) {
            var mA = new Vector(s.Begin.X - p.X, s.Begin.Y - p.Y);
            var mB = new Vector(s.End.X - p.X, s.End.Y - p.Y);
            if (GetLength(mA) < 1e-6 || GetLength(mB) < 1e-6) return true;
            else return Math.Abs(GetAngle(mA, mB) - Math.PI) < 1e-6;
        }
    }
}
