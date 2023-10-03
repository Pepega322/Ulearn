using System.Linq;

namespace Names {
    internal static class HeatmapTask {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names) {
            var days = Enumerable.Range(2, 30).Select(i => i.ToString()).ToArray();
            var month = Enumerable.Range(1, 12).Select(i => i.ToString()).ToArray();
            var heatmap = new double[30, 12];
            var groups = names
                .Where(p => p.BirthDate.Day != 1)
                .GroupBy(p => p.BirthDate.Month);

            foreach (var byMonth in groups)
                foreach (var byDay in byMonth.GroupBy(p => p.BirthDate.Day))
                    heatmap[byDay.Key - 2, byMonth.Key - 1] = byDay.Count();

            return new HeatmapData("Пример карты интенсивностей", heatmap, days, month);
        }
    }
}