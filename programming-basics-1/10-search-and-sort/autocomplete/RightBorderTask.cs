using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autocomplete
{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
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