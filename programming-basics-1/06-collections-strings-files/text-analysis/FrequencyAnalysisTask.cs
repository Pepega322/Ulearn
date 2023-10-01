using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var mostFrequentNextWords = new Dictionary<string, string>();
            var countedNGrams = GetCountedNGrams(text);
            var valueBuilder = new StringBuilder();

            foreach (var nGram in countedNGrams)
            {
                var key = nGram.Key;
                var value = GetMostFrequentNextWord(nGram.Value, valueBuilder);
                if (!mostFrequentNextWords.ContainsKey(key)) mostFrequentNextWords[key] = "";
                mostFrequentNextWords[key] = value;
            }

            return mostFrequentNextWords;
        }

        private static string GetMostFrequentNextWord(Dictionary<string, int> countValues, StringBuilder valueBuilder)
        {
            var maxFrequency = 0;

            foreach (var item in countValues)
            {
                var frequency = item.Value;
                if (frequency > maxFrequency)
                {
                    valueBuilder.Clear();
                    valueBuilder.Append(item.Key);
                    maxFrequency = frequency;
                    continue;
                }
                if (frequency == maxFrequency)
                {
                    var currentWord = valueBuilder.ToString();
                    valueBuilder.Clear();
                    var wordToAdd = string.CompareOrdinal(item.Key, currentWord) < 0 ? item.Key : currentWord;
                    valueBuilder.Append(wordToAdd);
                }
            }

            return valueBuilder.ToString();
        }

        static Dictionary<string, Dictionary<string, int>> GetCountedNGrams(List<List<string>> text)
        {
            var countedNGrams = new Dictionary<string, Dictionary<string, int>>();
            var minNGramSize = 2;
            var maxNGramSize = 3;
            var keyBuilder = new StringBuilder();

            foreach (var sentence in text)
            {
                for (var nGramSize = minNGramSize; nGramSize <= maxNGramSize; nGramSize++)
                {
                    for (var wordNumber = 0; wordNumber < sentence.Count - nGramSize + 1; wordNumber++)
                    {
                        var key = GetKey(keyBuilder, sentence, nGramSize, wordNumber);
                        var value = sentence[wordNumber + nGramSize - 1];

                        if (!countedNGrams.ContainsKey(key)) countedNGrams[key] = new Dictionary<string, int>();
                        if (!countedNGrams[key].ContainsKey(value)) countedNGrams[key][value] = 0;
                        countedNGrams[key][value]++;
                    }
                }
            }

            return countedNGrams;
        }

        private static string GetKey(StringBuilder keyBuilder, List<string> sentence, int nGramSize, int startWord)
        {
            for (var iteration = 0; iteration < nGramSize - 1; iteration++)
            {
                var wordToAdd = sentence[startWord + iteration];
                keyBuilder.Append(wordToAdd);
                if (iteration != nGramSize - 2) keyBuilder.Append(" ");
            }
            var key = keyBuilder.ToString();
            keyBuilder.Clear();
            return key;
        }
    }
}