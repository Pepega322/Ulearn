using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, 
            string phraseBeginning, int wordsCount)
        {
            if (phraseBeginning.Length == 0) return phraseBeginning;
            var phrase = new List<string>();
            var firstWords = phraseBeginning.Split(' ');
            for (var i = 0; i < firstWords.Length; i++)
                phrase.Add(firstWords[i]);

            while (wordsCount > 0)
            {
                var key = GetKey(phrase);
                wordsCount--;
                if (nextWords.ContainsKey(key))
                {
                    phrase.Add(nextWords[key]);
                    continue;
                }
                key = phrase[phrase.Count - 1];
                if (!nextWords.ContainsKey(key)) break;
                phrase.Add(nextWords[key]);
            }

            return string.Join(" ", phrase);
        }

        private static string GetKey(List<string> phrase)
        {
            if (phrase.Count == 1) return phrase[0];
            else return phrase[phrase.Count - 2] + " " + phrase[phrase.Count - 1];
        }
    }
}