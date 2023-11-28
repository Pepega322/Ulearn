using System;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator {
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents) {
        var result = new List<ComparisonResult>();
        for (var i = 0; i < documents.Count; i++)
            for (var j = i + 1; j < documents.Count; j++) {
                var distance = LevenshteinDistance(documents[i], documents[j]);
                var comparison = new ComparisonResult(documents[i], documents[j], distance);
                result.Add(comparison);
            }
        return result;
    }

    public double LevenshteinDistance(DocumentTokens first, DocumentTokens second) {
        var currentRow = new double[first.Count + 1];
        for (var i = 0; i <= first.Count; i++) 
            currentRow[i] = i;

        for (var row = 1; row <= second.Count; row++) {
            var nextRow = new double[currentRow.Length];
            nextRow[0] = row;
            for (var column = 1; column <= first.Count; column++) {
                var token1 = second[row - 1];
                var token2 = first[column - 1];
                if (token1 != token2) {
                    var distance = TokenDistanceCalculator.GetTokenDistance(token2, token1);
                    var ifRewrite = distance + currentRow[column - 1];
                    var ifReplace = 1 + Math.Min(nextRow[column - 1], currentRow[column]);
                    nextRow[column] = Math.Min(ifReplace, ifRewrite);
                }
                else nextRow[column] = currentRow[column - 1];
            }
            currentRow = nextRow;
        }
        return currentRow[first.Count];
    }
}