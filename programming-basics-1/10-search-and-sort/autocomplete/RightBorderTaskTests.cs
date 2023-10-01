using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Autocomplete
{
    [TestFixture]
    public class RightBorderTaskTests
    {

        [TestCase(new string[3] { "a", "ab", "abc" }, "abc", -1, 3, 3)]
        [TestCase(new string[4] { "ab", "ab", "ab", "ab" }, "a", -1, 4, 4)]
        [TestCase(new string[3] { "a", "ab", "abc" }, "aa", -1, 3, 1)]
        public void TestCases(string[] phrases, string prefix, int left, int right, int expectedResult)
        {
            var actualResult = GetRightBorderIndex(phrases, prefix, left, right);
            Assert.AreEqual(expectedResult, actualResult);
        }

        public static int GetRightBorderIndex(string[] phrases, string prefix, int left, int right)
        {
            if (prefix.Length == 0) return right;
            while (true)
            {
                if (right - left == 1) return right;
                var middle = (int)(((long)right + (long)left) / 2);
                var comparison = string.Compare(phrases[middle], prefix, StringComparison.OrdinalIgnoreCase);
                if (comparison > 0 && !phrases[middle].StartsWith(prefix)) right = middle;
                else left = middle;
            }
        }
    }
}
