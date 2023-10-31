namespace GeometryTasks {
    public class Segment {
        public readonly Vector Begin;
        public readonly Vector End;

        public double GetLength() => Geometry.GetLength(this);

        public bool Contains(Vector point) => Geometry.IsVectorInSegment(point, this);
    }
}
