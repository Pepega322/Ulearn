using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var buildWord = new StringBuilder();
            var separators = new[] { '.', '!', '?', ';', ':', '(', ')' };
            var sentences = text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var sentence in sentences)
            {
                var wordList = SentenceToList(sentence, buildWord);
                if (wordList.Count != 0) sentencesList.Add(wordList);
            }

            return sentencesList;
        }

        static List<string> SentenceToList(string sentence, StringBuilder buildWord)
        {
            var wordList = new List<string>();

            for (var i = 0; i < sentence.Length; i++)
            {
                if (char.IsLetter(sentence[i]) || sentence[i] == '\'')
                {
                    buildWord.Append(char.ToLower(sentence[i]));
                    if (i != sentence.Length - 1) continue;
                }
                if (buildWord.Length != 0)
                {
                    wordList.Add(buildWord.ToString());
                    buildWord.Clear();
                }
            }

            return wordList;
        }
    }
}