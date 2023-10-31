using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder {
    public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets) {
        var notVisitedTargets = targets.ToHashSet();
        var visited = new HashSet<Point>();
        var track = new Dictionary<Point, DijkstraData>();
        track[start] = new DijkstraData(start, 0);
        while (true) {
            var toOpen = GetCellToOpen(visited, track);
            if (toOpen == new Point(-1, -1)) yield break;
            if (notVisitedTargets.Contains(toOpen)) {
                notVisitedTargets.Remove(toOpen);
                var path = track[toOpen].Reverse().ToArray();
                yield return new PathWithCost(track[toOpen].Price, path);
            }

            foreach (var next in state.MoveAllDirections(toOpen)) {
                var price = track[toOpen].Price + state.GetCost(next);
                if (!track.ContainsKey(next) || price < track[next].Price)
                    track[next] = new DijkstraData(next, price, track[toOpen]);
            }
            visited.Add(toOpen);
        }
    }

    private Point GetCellToOpen(HashSet<Point> visited, Dictionary<Point, DijkstraData> track) {
        var toOpen = new Point(-1, -1);
        var bestPrice = int.MaxValue;
        foreach (var data in track.Values)
            if (!visited.Contains(data.Position) && data.Price < bestPrice) {
                toOpen = data.Position;
                bestPrice = data.Price;
            }
        return toOpen;
    }
}

public class DijkstraData : IEnumerable<Point> {
    public readonly Point Position;
    public int Price { get; set; }
    public DijkstraData Previous { get; set; }

    public DijkstraData(Point position, int price, DijkstraData previous) {
        Position = position;
        Price = price;
        Previous = previous;
    }

    public DijkstraData(Point position, int price) : this(position, price, null) { }

    public IEnumerator<Point> GetEnumerator() {
        var current = this;
        while (current != null) {
            yield return current.Position;
            current = current.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class StateExtensions {
    private static readonly Point[] possibleDirections = new[] {
        new Point(1, 0),
        new Point(-1, 0),
        new Point(0, 1),
        new Point(0, -1)
    };

    public static IEnumerable<Point> MoveAllDirections(this State s, Point start)
        => possibleDirections.Select(d => start + d).Where(p => s.InsideMap(p) && !s.IsWallAt(p));

    public static int GetCost(this State s, Point p)
        => s.InsideMap(p) ? s.CellCost[p.X, p.Y] : throw new ArgumentException("Outside map");
}