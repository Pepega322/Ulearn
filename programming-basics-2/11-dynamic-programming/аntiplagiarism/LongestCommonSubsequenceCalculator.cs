using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator {
    public static List<string> Calculate(List<string> first, List<string> second) {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    public static int[,] CreateOptimizationTable(List<string> first, List<string> second) {
        var table = new int[first.Count + 1, second.Count + 1];
        for (var row = 1; row <= first.Count; row++)
            for (var col = 1; col <= second.Count; col++) {
                table[row, col] = (first[row - 1] == second[col - 1]) ?
                    table[row - 1, col - 1] + 1
                    : Math.Max(table[row - 1, col], table[row, col - 1]);
            }
        return table;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second) {
        var result = new List<string>();

        var row = first.Count;
        var col = second.Count;
        var optimum = opt[first.Count, second.Count];
        while (optimum != 0) {
            while (opt[row - 1, col] == optimum || opt[row, col - 1] == optimum) {
                if (opt[row - 1, col] == optimum) row--;
                if (opt[row, col - 1] == optimum) col--;
            }
            result.Add(first[row - 1]);
            row--;
            col--;
            optimum--;
        }

        result.Reverse();
        return result;
    }
}