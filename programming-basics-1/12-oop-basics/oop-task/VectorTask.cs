using System;
using System.Runtime.CompilerServices;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector2)
        {
            return Geometry.Add(this, vector2);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector point)
        {
            return Geometry.IsVectorInSegment(point, this);
        }
    }

    public static class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            var vector = new Vector
            {
                X = segment.End.X - segment.Begin.X,
                Y = segment.End.Y - segment.Begin.Y
            };
            return GetLength(vector);
        }

        public static double GetSkew(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector2.X - vector1.Y;
        }

        public static double GetScalar(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static double GetAngle(Vector vector1, Vector vector2)
        {
            var length1 = GetLength(vector1);
            var length2 = GetLength(vector2);
            if (length1 < 1e-6 || length2 < 1e-6) throw new Exception("Zero vector");
            var scalar = GetScalar(vector1, vector2);
            return Math.Acos(scalar / (length1 * length2));
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector { X = vector1.X + vector2.X, Y = vector1.Y + vector2.Y };
        }

        public static bool IsVectorInSegment(Vector point, Segment segment)
        {
            var mA = new Vector
            {
                X = segment.Begin.X - point.X,
                Y = segment.Begin.Y - point.Y
            };
            var mB = new Vector
            {
                X = segment.End.X - point.X,
                Y = segment.End.Y - point.Y
            };
            if (GetLength(mA) < 1e-6 || GetLength(mB) < 1e-6) return true;
            else return Math.Abs(GetAngle(mA, mB) - Math.PI) < 1e-6;
        }
    }
}
