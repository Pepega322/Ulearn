using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var window = new Window<double>(windowWidth);
        foreach (var point in data)
        {
            window.Add(point.OriginalY);
            yield return point.WithMaxY(window.Max);
        }
    }
}

public class Window<T> where T : IComparable
{
    private readonly int size;
    private readonly Queue<T> items;
    private readonly LinkedList<T> maxItems;
    private T lastDelated;
    public T Max { get { return maxItems.First.Value; } }

    public Window(int windowSize)
    {
        size = windowSize;
        items = new Queue<T>();
        maxItems = new LinkedList<T>();
    }

    public void Add(T item)
    {
        items.Enqueue(item);
        if (items.Count > size)
            lastDelated = items.Dequeue();
        AddMax(item);
    }

    private void AddMax(T item)
    {
        if (maxItems.Count == 0)
        {
            maxItems.AddFirst(item);
            return;
        }

        if (maxItems.Count == size
            || lastDelated.CompareTo(maxItems.First.Value) == 0)
            maxItems.RemoveFirst();
        while (maxItems.Count != 0
            && item.CompareTo(maxItems.Last.Value) > 0)
            maxItems.RemoveLast();
        maxItems.AddLast(item);
    }
}