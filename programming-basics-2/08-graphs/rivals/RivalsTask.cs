using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask {
    public static IEnumerable<OwnedLocation> AssignOwners(Map map) {
        var owned = new HashSet<Point>();
        var queue = new Queue<OwnedLocation>();
        for (var i = 0; i < map.Players.Length; i++) {
            var start = new OwnedLocation(i, map.Players[i], 0);
            queue.Enqueue(start);
            owned.Add((start.Location));
            yield return start;
        }

        while (queue.Count != 0 && owned.Count != map.Maze.Length) {
            var current = queue.Dequeue();
            foreach (var next in map.GetNextLocations(current)) {
                if (owned.Contains(next.Location)) continue;
                owned.Add(next.Location);
                queue.Enqueue(next);
                yield return next;
            }
        }
    }
}

public static class RivalsExtensions {
    public static IEnumerable<Point> MoveDirections = new[] {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    public static IEnumerable<OwnedLocation> GetNextLocations(this Map map, OwnedLocation curent)
        => MoveDirections
        .Select(direction => curent.Location + direction)
        .Where(location => map.InBounds(location) && map.Maze[location.X, location.Y] != MapCell.Wall)
        .Select(location => new OwnedLocation(curent.Owner, location, curent.Distance + 1));
}