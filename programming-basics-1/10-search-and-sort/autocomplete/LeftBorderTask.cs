using System;
using System.Collections.Generic;

namespace Autocomplete;

public class LeftBorderTask {
    public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right) {
        if (right - left == 1) return left;
        var middle = (int)(((long)right + (long)left) / 2);
        var compare = string.Compare(phrases[middle], prefix, StringComparison.InvariantCultureIgnoreCase);
        if (compare < 0) left = middle;
        else right = middle;
        return GetLeftBorderIndex(phrases, prefix, left, right);
    }
}