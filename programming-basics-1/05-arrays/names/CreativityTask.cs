using System;
using System.Linq;

namespace Names {
    internal static class CreativityTask {
        public static HistogramData GetTop5PopularNames(NameData[] names) {
            var top = names
                .GroupBy(p => p.Name)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(g => Tuple.Create(g.Key, g.Count()));

            return new HistogramData("Топ 5 самых популярных имён", 
                top.Select(n => n.Item1).ToArray(), 
                top.Select(n => (double)n.Item2).ToArray());
        }
    }
}
