using System.Collections.Generic;

namespace LimitedSizeStack;

public interface ICommand
{
    ICommand Run();
}

public class AddItem<T> : ICommand
{
    public readonly ListModel<T> Object;
    public readonly T Item;

    public AddItem(ListModel<T> @object, T item)
    {
        Object = @object;
        Item = item;
    }

    public ICommand Run()
    {
        Object.Items.Add(Item);
        return this;
    }
}

public class RemoveItem<T> : ICommand
{
    public readonly ListModel<T> Object;
    public readonly T Item;
    public readonly int Index;

    public RemoveItem(ListModel<T> @object, int index)
    {
        Object = @object;
        Item = @object.Items[index];
        Index = index;
    }

    public ICommand Run()
    {
        Object.Items.RemoveAt(Index);
        return this;
    }
}

public class UndoChange<T> : ICommand
{
    public readonly ListModel<T> Object;

    public UndoChange(ListModel<T> @object)
    {
        Object = @object;
    }

    public ICommand Run()
    {
        var command = Object.Actions.Pop();
        if (command is AddItem<T>)
            Object.Items.RemoveAt(Object.Items.Count - 1);
        if (command is RemoveItem<T> temp)
            Object.Items.Insert(temp.Index, temp.Item);
        return this;
    }
}

public class ListModel<T>
{
    public List<T> Items { get; }
    public int UndoLimit;
    public LimitedSizeStack<ICommand> Actions { get; }

    public ListModel(int undoLimit) : this(new List<T>(), undoLimit)
    {
    }

    public ListModel(List<T> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        Actions = new LimitedSizeStack<ICommand>(undoLimit);
    }

    public void AddItem(T item)
    {
        var command = new AddItem<T>(this, item);
        Actions.Push(command.Run());
    }

    public void RemoveItem(int index)
    {
        var command = new RemoveItem<T>(this, index);
        Actions.Push(command.Run());
    }

    public bool CanUndo()
    {
        return Actions.Count != 0;
    }

    public void Undo()
    {
        var command = new UndoChange<T>(this);
        command.Run();
    }
}