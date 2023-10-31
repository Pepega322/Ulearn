using NUnit.Framework;

namespace PocketGoogle {
    [TestFixture]
    public class IndexerTests {
        [TestCase("C", new string[] { "A B C", "B C", "A C A C" }, new int[] { 0, 1, 2 })]
        [TestCase("ff", new string[] { "F, f ff" }, new int[] { 0 })]
        public void GetIdsTest(string word, string[] texts, int[] expectedResult) {
            var indexer = new Indexer();
            for (var i = 0; i < texts.Length; i++)
                indexer.Add(i, texts[i]);
            var actualResult = indexer.GetIds(word).ToArray();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase("f", 0, new string[] { "F, f ff" }, new int[] { 3 })]
        public void GetPositionsTest(string word, int id, string[] texts, int[] expectedResult) {
            var indexer = new Indexer();
            for (var i = 0; i < texts.Length; i++)
                indexer.Add(i, texts[i]);
            var actualResult = indexer.GetPositions(id, word).ToArray();
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
