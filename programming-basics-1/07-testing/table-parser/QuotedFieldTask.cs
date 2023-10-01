using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("\"\"", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'x y'", 0, "x y", 5)]
        [TestCase("\"bcd ef\"", 0, "bcd ef", 8)]
        [TestCase("\"def g h", 0, "def g h", 8)]
        [TestCase("\"a 'b' 'c' d\"", 0, "a 'b' 'c' d", 13)]
        [TestCase("'\"1\" \"2\" \"3\"'", 0, "\"1\" \"2\" \"3\"", 13)]
        [TestCase("\"QF \\\"\"", 0, "QF \"", 7)]

        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var countOpenQuote = IsEndQuoteExist(line, startIndex) ? 2 : 1;
            var countSlash = CountSlash(line, startIndex);
            var value = GetQuotedTokenValue(line, startIndex);
            var length = value.Length + countSlash + countOpenQuote;

            return new Token(value, startIndex, length);
        }

        private static string GetQuotedTokenValue(string line, int startIndex)
        {
            var valueBuilder = new StringBuilder();
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == line[startIndex]) break;
                if (line[i] == '\\')
                {
                    valueBuilder.Append(line[i + 1]);
                    i++;
                    continue;
                }
                valueBuilder.Append(line[i]);
            }

            return valueBuilder.ToString();
        }

        private static int CountSlash(string line, int startIndex)
        {
            var countSlash = 0;
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == line[startIndex] && line[i - 1] != '\\') break;
                if (line[i] == '\\') countSlash++;
            }

            return countSlash;
        }

        private static bool IsEndQuoteExist(string line, int startIndex)
        {
            var isEndQuoteExist = false;
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == line[startIndex] && line[i - 1] != '\\')
                {
                    isEndQuoteExist = true;
                    break;
                }
            }

            return isEndQuoteExist;
        }
    }
}
