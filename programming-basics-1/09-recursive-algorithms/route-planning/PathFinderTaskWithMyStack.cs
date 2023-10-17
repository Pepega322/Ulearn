using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning;
//как вы уже знаете из курса, у нас есть стек, а есть куча
//и в рекурсии мы используем стек, который ограничен по размерам
//если же реализовать всё как в варианте ниже - мы будем использовать стек,
//который как все другие экземпляры классов лежит в куче
//это может быть полезно, если нужно совершить ооочень большой перебор
//при обычной рекурсии может выскочить StackOverflow,
//даже если вы правильно написали выход из рекурсии

//не забудь преименовать класс
public static class PathFinderTaskWithMyStack {
    public static int[] FindBestCheckpointsOrder(Point[] checkpoints) {
        var bestOrder = Enumerable.Range(0, checkpoints.Length).ToArray();
        var bestLength = checkpoints.GetPathLength(bestOrder);
        //эта строчка нужна для проверки на рекурсию на сайте
        FindBestCheckpointsOrder(checkpoints, bestOrder, 1);

        var stack = new Stack<MyPath1>();
        stack.Push(CreateStartPath(checkpoints));
        while (stack.Count != 0) {
            var p = stack.Pop();
            if (p.IsComplete()) {
                bestOrder = p.GetVisitOrder().ToArray();
                bestLength = p.Length;
                continue;
            }

            for (var i = 1; i < checkpoints.Length; i++) {
                var path = p.Copy();
                if (path.IsVisited(i)) continue;
                path.AddPoint(i);
                if (path.Length - bestLength > 1e-6) continue;
                else stack.Push(path);
            }
        }
        return bestOrder;
    }

    public static MyPath1 CreateStartPath(Point[] checkpoints) {
        var path = new MyPath1(checkpoints);
        path.AddPoint(0);
        return path;
    }

    private static void FindBestCheckpointsOrder(Point[] checkpoints, int[] order, int position) {
    }
}

public class MyPath1 {
    private readonly Point[] points;
    private readonly List<int> visitOrder;
    private readonly HashSet<int> visited;
    public double Length { get; private set; }

    public MyPath1(Point[] points, List<int> visitOrder, HashSet<int> visited, double length) {
        this.points = points;
        this.visitOrder = visitOrder;
        this.visited = visited;
        Length = length;
    }

    public MyPath1(Point[] points) : this(points, new List<int>(), new HashSet<int>(), 0) {
    }

    public override string ToString() {
        return $"L = {Length} Path: " + string.Join("-", visitOrder);
    }

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

    public MyPath1 Copy()
        => new MyPath1(points, visitOrder.ToList(), visited.ToHashSet(), Length);
}