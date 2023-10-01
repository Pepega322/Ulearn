using System;
using System.Linq;

namespace Names
{
    internal static class CreativityTask
    {
        public static HistogramData GetHistogramBirthsByMounth(NameData[] names)
        {
            var mounth = new string[12];
            for (var i = 0; i < mounth.Length; i++)
                mounth[i] = (1 + i).ToString();

            var birthCounts = new double[12];
            for (var i = 0; i < birthCounts.Length; i++)
            {
                foreach (var name in names)
                {
                    if (name.BirthDate.Month == i + 1) birthCounts[i]++;
                }
            }
            return new HistogramData("Рождаемость по месяцам", mounth, birthCounts);
        }
    }
}
