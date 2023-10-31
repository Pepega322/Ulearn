using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask {
    public static MoveDirection[] FindShortestPath(Map map) {
        var shortest = FindShortestPat(map);
        if (shortest == null) return new MoveDirection[0];
        var path = new List<MoveDirection>();
        for (var i = 1; i < shortest.Count; i++)
            path.Add(Walker.ConvertOffsetToDirection(shortest[i] - shortest[i - 1]));
        return path.ToArray();
    }

    public static List<Point>? FindShortestPat(Map map) {
        var exitToStart = BfsTask.FindPaths(map, map.Exit, new[] { map.InitialPosition }).FirstOrDefault();
        if (exitToStart == default) return null;

        var exitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests);
        var startToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
        var paths = from exitToChest in exitToChests
                    join startToChest in startToChests 
                    on exitToChest.Value equals startToChest.Value
                    select new {
                        StartToChest = startToChest,
                        ExitToChest = exitToChest,
                        Length = exitToChest.Length + startToChest.Length - 1
                    };
        if (!paths.Any()) return exitToStart.ToList();

        var shortest = paths.MinBy(p => p.Length);
        var result = shortest.StartToChest.ToList();
        result.Reverse();
        result.AddRange(shortest.ExitToChest.Skip(1));
        return result;
    }
}