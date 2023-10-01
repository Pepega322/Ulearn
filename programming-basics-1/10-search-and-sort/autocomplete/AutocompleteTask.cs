using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count)
                return phrases[index];

            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var words = new List<string>();
            for (var i = 0; i < count; i++)
            {
                if (index + i >= phrases.Count ||
                    !phrases[index + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    break;
                words.Add(phrases[index + i]);
            }
            return words.ToArray();
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var rightIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
            var count = rightIndex - leftIndex + 1;
            return count;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [TestCase(new string[1] { "aa" }, "a", 2, new string[1] { "aa" })]
        [TestCase(new string[3] { "aa", "ab", "ac" }, "z", 2, new string[0])]
        public void TopByPrefix_IsEmpty_WhenNoPhrases(string[] phrases, string prefix, int count, string[] expectedResult)
        {
            var actualResult = GetTopByPrefix(phrases, prefix, count);
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        public static string[] GetTopByPrefix(string[] phrases, string prefix, int count)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Length) + 1;
            var words = new List<string>();
            for (var i = 0; i < count; i++)
            {
                if (index + i >= count ||
                    !phrases[index + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    break;
                words.Add(phrases[index + i]);
            }

            return words.ToArray();
        }

        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "a", 2)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "c", 2)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "d", 0)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "6", 0)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "", 7)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "cb", 1)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "aa", 1)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "cz", 0)]
        [TestCase(new string[7] { "aa", "ab", "bc", "bd", "be", "ca", "cb" }, "z", 0)]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(string[] phrases, string prefix, int expectedCount)
        {
            var actualCount = GetCountByPrefix(phrases, prefix);
            Assert.AreEqual(expectedCount, actualCount);
        }

        public static int GetCountByPrefix(string[] phrases, string prefix)
        {
            var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Length) + 1;
            var rightIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Length) - 1;
            return rightIndex - leftIndex + 1;
        }
    }
}
