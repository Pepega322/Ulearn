namespace GeometryTasks {
    public class Vector {
        public readonly double X;
        public readonly double Y;

        public Vector() { }

        public Vector(double x, double y) {
            X = x;
            Y = y;
        }

        public double GetLength() => Geometry.GetLength(this);

        public Vector Add(Vector vector2) => Geometry.Add(this, vector2);

        public bool Belongs(Segment segment) => Geometry.IsVectorInSegment(this, segment);
    }
}
