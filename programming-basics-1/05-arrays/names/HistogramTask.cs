using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var days = new string[31];
            for (int i = 0; i < days.Length; i++)
                days[i] = (1 + i).ToString();

            var birthPerDay = new double[31];
            for (var day = 0; day < days.Length; day++)
            {
                foreach (var people in names)
                {
                    if (people.Name == name &&
                        people.BirthDate.Day == (day + 1) &&
                        people.BirthDate.Day != 1) birthPerDay[day]++;
                }
            }

            return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name), days, birthPerDay);
        }
    }
}