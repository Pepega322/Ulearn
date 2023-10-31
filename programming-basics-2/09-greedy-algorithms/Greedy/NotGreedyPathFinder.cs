using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder {
    public List<Point> FindPathToCompleteGoal(State state) {
        var stack = new Stack<PathBuilder>();
        var best = new PathBuilder(state.Position, state.InitialEnergy);
        stack.Push(best);
        var bestCost = int.MaxValue;
        while (stack.Count != 0) {
            var current = stack.Pop();
            if (current.TargetsCount > best.TargetsCount) {
                best = current;
                bestCost = current.Cost;
                if (best.TargetsCount == state.Chests.Count) break;
            }

            foreach (var target in state.Chests) {
                if (current.ContainsTarget(target)) continue;
                var next = current.AddTarget(state, target);
                if (next == null) continue;
                if (next.Cost <= bestCost || next.TargetsCount >= best.TargetsCount)
                    stack.Push(next);
            }
        }
        return best.Path;
    }
}

public class PathBuilder {
    public readonly static DijkstraPathFinder PathFinder = new();

    public readonly int EnergyLimit;
    private readonly HashSet<Point> targets;
    public readonly List<Point> Path;
    public Point Position { get; private set; }
    public int Cost { get; private set; }
    public int TargetsCount => targets.Count;

    public PathBuilder(Point start, int energyLimit) {
        EnergyLimit = energyLimit;
        targets = new HashSet<Point>();
        Path = new List<Point>();
        Position = start;
    }

    private PathBuilder(PathBuilder reference) {
        EnergyLimit = reference.EnergyLimit;
        targets = reference.targets.ToHashSet();
        Path = reference.Path.ToList();
        Position = reference.Position;
        Cost = reference.Cost;
    }

    public bool ContainsTarget(Point target) => targets.Contains(target);

    public PathBuilder? AddTarget(State map, Point target) {
        if (!map.Chests.Contains(target)) return null;
        var path = PathFinder.GetPathsByDijkstra(map, Position, new[] { target }).FirstOrDefault();
        if (path == default || Cost + path.Cost > EnergyLimit) return null;
        var result = new PathBuilder(this);
        result.targets.Add(target);
        result.Path.AddRange(path.Path.Skip(1));
        result.Position = path.End;
        result.Cost = Cost + path.Cost;
        return result;
    }
}