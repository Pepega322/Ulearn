using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                result.Add(string.Join(string.Empty, word));
                return;
            }

            var symbol = word[startIndex];
            bool isLetter = char.IsLetter(symbol);
            bool haveUpperCase = char.Equals(char.ToLower(symbol), char.ToUpper(symbol));
            if (isLetter && !haveUpperCase)
            {
                word[startIndex] = char.ToLower(symbol);
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = char.ToUpper(symbol);
                AlternateCharCases(word, startIndex + 1, result);
            }
            else
            {
                AlternateCharCases(word, startIndex + 1, result);
            }
        }
    }
}