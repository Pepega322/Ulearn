using System;
using System.Collections.Generic;

namespace Autocomplete;

public class RightBorderTask {
    public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right) {
        if (prefix.Length == 0) return right;
        while (true) {
            if (right - left == 1) return right;
            var middle = (int)(((long)right + (long)left) / 2);
            var compare = string.Compare(phrases[middle], prefix, StringComparison.InvariantCultureIgnoreCase);
            if (compare > 0 && !phrases[middle].StartsWith(prefix)) right = middle;
            else left = middle;
        }
    }
}