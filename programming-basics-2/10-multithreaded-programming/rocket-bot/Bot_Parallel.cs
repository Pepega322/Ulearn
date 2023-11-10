using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot {
    public Rocket GetNextMove(Rocket rocket) {
        var tasks = CreateTasks(rocket);
        var (bestTurn, bestScore) = tasks.MaxBy(t => t.Result.Score).Result;
        return rocket.Move(bestTurn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket) {
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        for (var i = 0; i < threadsCount; i++) {
            var rnd = new Random(random.Next());
            tasks.Add(Task.Run(() => SearchBestMove(rocket, rnd, iterationsCount / threadsCount)));
        }
        return tasks;
    }
}