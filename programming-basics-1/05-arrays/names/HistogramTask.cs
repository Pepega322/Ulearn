using System.Linq;

namespace Names {
    internal static class HistogramTask {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] data, string name) {
            var days = Enumerable.Range(1, 31).Select(i => i.ToString()).ToArray();
            var people = data
                .Where(p => p.Name == name)
                .GroupBy(p => p.BirthDate.Day);

            var birthPerDar = new double[31];
            foreach (var group in people) {
                if (group.Key == 1) continue;
                birthPerDar[group.Key - 1] = group.Count();
            }

            return new HistogramData($"Рождаемость людей с именем '{name}'", days, birthPerDar);
        }
    }
}