using System;
using System.Collections.Generic;

namespace Autocomplete;

internal class AutocompleteTask {
    public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix) {
        var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        return (index + 1 < phrases.Count) ? phrases[index + 1] : string.Empty;
    }

    public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count) {
        var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var words = new List<string>();
        for (var i = index + 1; i < phrases.Count; i++) {
            if (count == 0 ||
                !phrases[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                break;
            words.Add(phrases[i]);
            count--;
        }
        return words.ToArray();

        //такой вариант выглядит конечно приятнее,
        //но увы - он сильно проигрывает в производительности варианту с циклом
        //может позже рабирусь со сложностью каждого LINQ метода и будет понятнее
        //return phrases
        //    .Skip(index+1)
        //    .Take(count)
        //    .TakeWhile(p => p.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        //    .ToArray();
    }

    public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix) {
        var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
        return right - left - 1;
    }
}

