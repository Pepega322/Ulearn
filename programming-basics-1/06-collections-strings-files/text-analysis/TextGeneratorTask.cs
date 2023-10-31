using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis {
    static class TextGeneratorTask {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, 
            string phraseBeginning, int wordsCount) {
            if (phraseBeginning.Length == 0) return phraseBeginning;
            var phrase = phraseBeginning.Split(' ').ToList();
            while (wordsCount > 0) {
                var key = string.Join(" ", phrase.Skip(phrase.Count - 2));
                wordsCount--;
                if (nextWords.ContainsKey(key)) {
                    phrase.Add(nextWords[key]);
                    continue;
                }
                key = phrase.Last();
                if (!nextWords.ContainsKey(key)) break;
                phrase.Add(nextWords[key]);
            }
            return string.Join(" ", phrase);
        }
    }
}