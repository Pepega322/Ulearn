using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis {
    static class FrequencyAnalysisTask {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> sentences) {
            var nGrams = new NGramsFrequency();
            nGrams.AddNGramm(sentences, 2);
            nGrams.AddNGramm(sentences, 3);

            var words = new Dictionary<string, string>();
            foreach (var nGram in nGrams) {
                var start = nGram.Key;
                var end = nGram.Value
                     .OrderByDescending(p => p.Value)
                     .ThenBy(p => p.Key)
                     .First().Key;
                if (!words.ContainsKey(start)) words[start] = string.Empty;
                words[start] = end;
            }
            return words;
        }
    }

    public class NGramsFrequency : IEnumerable<KeyValuePair<string, Dictionary<string, int>>> {
        private readonly Dictionary<string, Dictionary<string, int>> nGrams;

        public NGramsFrequency() {
            nGrams = new Dictionary<string, Dictionary<string, int>>();
        }

        public void AddNGramm(List<List<string>> sentences, int n) {
            foreach (var sentence in sentences)
                for (var pos = 0; pos < sentence.Count - n + 1; pos++) {
                    var start = string.Join(" ", sentence.Skip(pos).Take(n - 1));
                    var end = sentence[pos + n - 1];
                    if (!nGrams.ContainsKey(start)) nGrams[start] = new Dictionary<string, int>();
                    if (!nGrams[start].ContainsKey(end)) nGrams[start][end] = 0;
                    nGrams[start][end]++;
                }
        }

        public IEnumerator<KeyValuePair<string, Dictionary<string, int>>> GetEnumerator() {
            foreach (var nGram in nGrams)
                yield return nGram;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}