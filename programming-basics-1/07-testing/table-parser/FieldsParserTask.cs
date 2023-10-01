using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, params string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase(@"")]
        [TestCase(@"a ", "a")]
        [TestCase(@"a b", "a", "b")]
        [TestCase(@"a  b", "a", "b")]
        [TestCase(@"'a' b", "a", "b")]
        [TestCase(@"a 'b'", "a", "b")]
        [TestCase(@"'a'", @"a")]
        [TestCase(@"""'a'""", @"'a'")]
        [TestCase(@"'""a""'", @"""a""")]
        [TestCase(@"''", @"")]
        [TestCase(@"'a b'", "a b")]
        [TestCase(@"'a'b", "a", "b")]
        [TestCase(@"'a", "a")]
        [TestCase(@"'a ", "a ")]
        [TestCase(@"'\''", @"'")]
        [TestCase(@"""\""""", @"""")]
        [TestCase(@"'\\'", @"\")]

        public static void RunTests(string input, params string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokenList = new List<Token>();
            var position = 0;
            while (position < line.Length)
            {
                var token = ReadField(line, position);
                if (token.Length != 0)
                {
                    tokenList.Add(token);
                    position += token.Length;
                    continue;
                }
                position++;
            }

            return tokenList;
        }

        private static Token ReadField(string line, int startIndex)
        {
            var quotes = new[] { '\"', '\'' };
            if (quotes.Contains(line[startIndex])) return ReadQuotedField(line, startIndex);
            var value = GetUnquotedTokenValue(line, startIndex, quotes);

            return new Token(value, startIndex, value.Length);
        }

        private static string GetUnquotedTokenValue(string line, int startIndex, char[] quotes)
        {
            var valueBuilder = new StringBuilder();
            for (var i = startIndex; i < line.Length; i++)
            {
                if (char.IsWhiteSpace(line[i]) || quotes.Contains(line[i])) break;
                valueBuilder.Append(line[i]);
            }

            return valueBuilder.ToString();
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}