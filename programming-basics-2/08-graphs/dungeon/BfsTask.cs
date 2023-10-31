using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask {
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests) {
        var unvisited = chests.ToHashSet();
        var path = new Dictionary<Point, SinglyLinkedList<Point>>();
        path[start] = new SinglyLinkedList<Point>(start);

        var queue = new Queue<Point>();
        queue.Enqueue(start);
        while (queue.Count != 0 && unvisited.Count != 0) {
            var current = queue.Dequeue();
            foreach (var next in current.GetNextPositions()) {
                if (!map.IsReachable(next) || path.ContainsKey(next)) continue;
                path[next] = new SinglyLinkedList<Point>(next, path[current]);
                queue.Enqueue(next);
            }

            if (unvisited.Contains(current)) {
                unvisited.Remove(current);
                yield return path[current];
            }
        }
    }
}

public static class BsfExtensions {
    public static IEnumerable<Point> GetNextPositions(this Point start)
        => Walker.PossibleDirections.Select(d => start + d);

    public static bool IsReachable(this Map map, Point p)
        => map.InBounds(p) && map.Dungeon[p.X, p.Y] == MapCell.Empty;
}