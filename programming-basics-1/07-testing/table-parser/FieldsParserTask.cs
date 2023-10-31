using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser {
    [TestFixture]
    public class FieldParserTaskTests {
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
        public static void RunTests(string input, params string[] expectedResult) {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
        }
    }

    public class FieldsParserTask {
        private static char[] quotes = new[] { '\"', '\'' };

        public static List<Token> ParseLine(string line) {
            var tokens = new List<Token>();
            var i = 0;
            while (i < line.Length) {
                var token = ReadField(line, i);
                if (token.Length == 0) i++;
                else {
                    tokens.Add(token);
                    i += token.Length;
                }
            }
            return tokens;
        }

        private static Token ReadField(string line, int startIndex) {
            if (quotes.Contains(line[startIndex]))
                return QuotedFieldTask.ReadQuotedField(line, startIndex);

            var value = GetUnquotedTokenValue(line, startIndex);
            return new Token(value, startIndex, value.Length);
        }

        private static string GetUnquotedTokenValue(string line, int startIndex) {
            var b = new StringBuilder();
            for (var i = startIndex; i < line.Length; i++) {
                if (char.IsWhiteSpace(line[i]) || quotes.Contains(line[i])) break;
                b.Append(line[i]);
            }
            return b.ToString();
        }
    }
}