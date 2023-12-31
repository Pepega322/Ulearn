﻿using System;
using System.IO;
using System.Linq;

namespace Names {
    public static class Program {
        private static readonly string dataFilePath = "names.txt";

        private static void Main(string[] args) {
            var namesData = ReadData();
            //Charts.ShowHeatmap(HeatmapTask.GetBirthsPerDateHeatmap(namesData));
            //Charts.ShowHistogram(HistogramSample.GetHistogramBirthsByYear(namesData));
            //Charts.ShowHistogram(HistogramTask.GetBirthsPerDayHistogram(namesData, "юрий"));
            //Charts.ShowHistogram(HistogramTask.GetBirthsPerDayHistogram(namesData, "максим"));
            Charts.ShowHistogram(CreativityTask.GetTop5PopularNames(namesData));
            var str = new string[3, 2];
            str[0, 0] = "a";
            str[0, 1] = "2";
            str[1, 0] = "b";
            str[1, 1] = "10";
            str[2, 0] = "c";
            str[2, 1] = "5";

            Console.WriteLine();
        }


        private static NameData[] ReadData() {
            var lines = File.ReadAllLines(dataFilePath);
            var names = new NameData[lines.Length];
            for (var i = 0; i < lines.Length; i++)
                names[i] = NameData.ParseFrom(lines[i]);
            return names;
        }

        // А это более короткая версия ReadData(). Она использует механизм языка под названием Linq
        // Вы можете познакомиться с ней самостоятельно: https://ulearn.azurewebsites.net/Course/Linq
        // Освоив LINQ решать задачи подобные NamesTask становится гораздо проще и приятнее.
        // Но это уже совсем другая история.
        private static NameData[] ReadData2() {
            return File
                .ReadLines(dataFilePath)
                .Select(NameData.ParseFrom)
                .ToArray();
        }
    }
}