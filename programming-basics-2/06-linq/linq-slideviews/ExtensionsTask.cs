using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask {
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items) {
        var array = items.OrderBy(f => f).ToArray();
        if (array.Length == 0) throw new InvalidOperationException();
        if (array.Length % 2 != 0) return array[array.Length / 2];
        else return (array[array.Length / 2] + array[array.Length / 2 - 1]) / 2;
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items) {
        var queue = new Queue<T>();
        foreach (var item in items) {
            queue.Enqueue(item);
            if (queue.Count == 2)
                yield return (queue.Dequeue(), queue.First());
        }
    }
}