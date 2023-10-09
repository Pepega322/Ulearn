using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalysis {
    static class SentencesParserTask {
        private static StringBuilder builder = new StringBuilder();

        public static List<List<string>> ParseSentences(string text) {
            var separators = new[] { '.', '!', '?', ';', ':', '(', ')' };
            var sentences = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return sentences
                .Select(s => SentenceToList(s))
                .Where(l => l.Count != 0)
                .ToList();
        }

        private static List<string> SentenceToList(string sentence) {
            var words = new List<string>();
            for (var i = 0; i < sentence.Length; i++) {
                if (char.IsLetter(sentence[i]) || sentence[i] == '\'') {
                    builder.Append(char.ToLower(sentence[i]));
                    if (i != sentence.Length - 1) continue;
                }
                if (builder.Length != 0) {
                    words.Add(builder.ToString());
                    builder.Clear();
                }
            }
            return words;
        }
    }
}