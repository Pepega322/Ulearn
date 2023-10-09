using System.Text;
using NUnit.Framework;

namespace TableParser {
    [TestFixture]
    public class QuotedFieldTaskTests {
        [TestCase("''", 0, "", 2)]
        [TestCase("\"\"", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'x y'", 0, "x y", 5)]
        [TestCase("\"bcd ef\"", 0, "bcd ef", 8)]
        [TestCase("\"def g h", 0, "def g h", 8)]
        [TestCase("\"a 'b' 'c' d\"", 0, "a 'b' 'c' d", 13)]
        [TestCase("'\"1\" \"2\" \"3\"'", 0, "\"1\" \"2\" \"3\"", 13)]
        [TestCase("\"QF \\\"\"", 0, "QF \"", 7)]

        public void Test(string line, string expectedValue, int startIndex, int expectedLength) {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    public class QuotedFieldTask {
        public static Token ReadQuotedField(string line, int startIndex) {
            var quoteCount = 1;
            var slashCount = 0;
            for (var i = startIndex + 1; i < line.Length; i++) {
                if (line[i] == '\\') slashCount++;
                if (line[i] == line[startIndex] && line[i - 1] != '\\') {
                    quoteCount = 2;
                    break;
                }
            }

            var value = GetQuotedTokenValue(line, startIndex);
            var length = value.Length + slashCount + quoteCount;
            return new Token(value, startIndex, length);
        }

        private static string GetQuotedTokenValue(string line, int startIndex) {
            var b = new StringBuilder();
            var i = startIndex + 1;
            while (i < line.Length && line[i] != line[startIndex]) {
                if (line[i] == '\\') {
                    b.Append(line[i + 1]);
                    i += 2;
                    continue;
                }
                b.Append(line[i]);
                i++;
            }
            return b.ToString();
        }
    }
}
