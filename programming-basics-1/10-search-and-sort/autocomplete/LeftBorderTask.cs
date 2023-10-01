using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (right - left <= 1) return left;
            var middle = (int)(((long)right + (long)left) / 2);
            var comparison = string.Compare(phrases[middle], prefix, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0) return GetLeftBorderIndex(phrases, prefix, middle, right);
            else return GetLeftBorderIndex(phrases, prefix, left, middle);
        }
    }
}
