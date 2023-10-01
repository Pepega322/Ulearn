using System;
using System.Linq;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var days = new string[30];
            for (int i = 0; i < days.Length; i++)
                days[i] = (2 + i).ToString();

            var mounth = new string[12];
            for (int i = 0; i < mounth.Length; i++)
                mounth[i] = (1 + i).ToString();

            var countBirth = new double[30, 12];
            for (var i = 0; i < 30; i++)
                for (var j = 0; j < 12; j++)
                {
                    foreach (var name in names)
                        if (name.BirthDate.Month == (j + 1) && name.BirthDate.Day == (i + 2)) countBirth[i, j]++;
                }

            return new HeatmapData("Пример карты интенсивностей", countBirth, days, mounth);
        }
    }
}