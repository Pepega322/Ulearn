using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder {
    public List<Point> FindPathToCompleteGoal(State state) {
        var result = new List<Point>();
        if (state.Chests.Count < state.Goal) return result;
        var dijkstra = new DijkstraPathFinder();
        var chests = state.Chests.ToHashSet();
        var energy = state.InitialEnergy;
        var position = state.Position;
        var score = 0;
        while (score != state.Goal) {
            var path = dijkstra.GetPathsByDijkstra(state, position, chests).FirstOrDefault();
            if (path == default || energy - path.Cost < 0) return result;
            result.AddRange(path.Path.Skip(1));
            chests.Remove(path.End);
            score++;
            energy -= path.Cost;
            position = path.End;
        }
        return result;
    }
}