using System;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    public int Count { get; private set; }
    public readonly int MaxSize;
    private readonly T[] Items;
    private int nextItem;
    private int topItem;

    public LimitedSizeStack(int maxSize)
    {
        MaxSize = maxSize;
        Items = new T[maxSize];
        nextItem = 0;
        topItem = 0;
    }

    public void Push(T item)
    {
        if (MaxSize == 0) return;
        Items[nextItem] = item;
        if (Count < MaxSize) Count++;
        if (nextItem == MaxSize - 1) nextItem = 0;
        else nextItem++;
    }

    public T Pop()
    {
        if (Count == 0) throw new Exception("Stack is empty");
        if (nextItem == 0) topItem = MaxSize - 1;
        else topItem = nextItem - 1;
        nextItem = topItem;
        Count--;
        return Items[topItem];
    }
}