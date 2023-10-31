using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning;

public static class PathFinderTask {
    private static int[] bestOrder;

    public static int[] FindBestCheckpointsOrder(Point[] checkpoints) {
        bestOrder = Enumerable.Range(0, checkpoints.Length).ToArray();
        var bestLength = checkpoints.GetPathLength(bestOrder);
        var start = new MyPath(checkpoints);
        start.AddPoint(0);
        FindBestOrder(start, ref bestLength);
        return bestOrder;
    }

    private static void FindBestOrder(MyPath path1, ref double bestLength) {
        if (path1.IsComplete()) {
            bestOrder = path1.GetVisitOrder().ToArray();
            bestLength = path1.Length;
            return;
        }

        for (var i = 1; i < bestOrder.Length; i++) {
            var path = path1.Copy();
            if (path.IsVisited(i)) continue;
            path.AddPoint(i);
            if (path.Length - bestLength > 1e-6) continue;
            else FindBestOrder(path, ref bestLength);
        }
    }
}


public class MyPath {
    private readonly Point[] points;
    private readonly List<int> visitOrder;
    private readonly HashSet<int> visited;
    public double Length { get; private set; }

    public MyPath(Point[] points, List<int> visitOrder, HashSet<int> visited, double length) {
        this.points = points;
        this.visitOrder = visitOrder;
        this.visited = visited;
        Length = length;
    }

    public MyPath(Point[] points) : this(points, new List<int>(), new HashSet<int>(), 0) {
    }

    public override string ToString()
        => $"L = {Length} Path: " + string.Join("-", visitOrder);

    public bool IsVisited(int pointIndex) => visited.Contains(pointIndex);

    public bool IsComplete() => visited.Count == points.Length;

    public List<int> GetVisitOrder() => visitOrder.ToList();

    public void AddPoint(int pointIndex) {
        visitOrder.Add(pointIndex);
        if (visited.Count != 0) {
            var prevPoint = points[visitOrder[visitOrder.Count - 2]];
            var currPoint = points[visitOrder[visitOrder.Count - 1]];
            Length += prevPoint.DistanceTo(currPoint);
        }
        visited.Add(pointIndex);
    }

    public MyPath Copy()
        => new MyPath(points, visitOrder.ToList(), visited.ToHashSet(), Length);
}
