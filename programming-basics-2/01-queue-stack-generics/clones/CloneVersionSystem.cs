using System;
using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    readonly List<Clone> Clones;

    public CloneVersionSystem()
    {
        Clones = new List<Clone>();
        Clones.Add(new Clone());
    }

    private Command ReadCommand(string query)
    {
        var info = query.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (info.Length < 2) throw new Exception("Incorrect command");
        var command = info[0];
        var cloneNumber = int.Parse(info[1]);
        if (command == "learn")
        {
            if (info.Length != 3)
                throw new Exception("Command \"learn\" must contains 2 parameters");
            return new Command(command, cloneNumber, info[2]);
        }
        return new Command(command, cloneNumber);
    }

    public string Execute(string query)
    {
        var command = ReadCommand(query);
        while (Clones.Count < command.CloneNumber) 
            Clones.Add(new Clone());
        var clone = Clones[command.CloneNumber - 1];
        return command.Name switch
        {
            "learn" => clone.Learn(command.Programm),
            "rollback" => clone.RollBack(),
            "relearn" => clone.ReLearn(),
            "clone" => clone.CreateCloneAt(Clones),
            "check" => clone.Check(),
            _ => throw new Exception(),
        };
    }
}

public class Command
{
    public readonly string Name;
    public readonly int CloneNumber;
    public readonly string Programm;

    public Command(string name, int cloneNumber)
    {
        Name = name;
        CloneNumber = cloneNumber;
    }

    public Command(string name, int cloneNumber, string programm)
    {
        Name = name;
        CloneNumber = cloneNumber;
        Programm = programm;
    }
}

public class Clone
{
    public readonly LinkedStack<string> Programms;
    public readonly LinkedStack<string> Rollbacks;
    private Clone(Clone reference)
    {
        Programms = reference.Programms.CreateCopy();
        Rollbacks = reference.Rollbacks.CreateCopy();
    }

    public Clone()
    {
        Programms = new LinkedStack<string>();
        Rollbacks = new LinkedStack<string>();
    }

    public string Learn(string programmNumber)
    {
        var programm = new StackItem<string>(programmNumber);
        Programms.Push(programm);
        Rollbacks.Clear();
        return null;
    }

    public string RollBack()
    {
        if (Programms.Count == 0) return null;
        var programm = Programms.Pop();
        Rollbacks.Push(programm);
        return null;
    }

    public string ReLearn()
    {
        if (Rollbacks.Count == 0) return null;
        var programm = Rollbacks.Pop();
        Programms.Push(programm);
        return null;
    }

    public string CreateCloneAt(List<Clone> clones)
    {
        var clone = new Clone(this);
        clones.Add(clone);
        return null;
    }

    public string Check()
    {
        if (Programms.Count == 0) return "basic";
        return Programms.Last.Value.ToString();
    }
}

public class StackItem<T>
{
    public readonly T Value;
    public readonly StackItem<T> Previous;

    public StackItem(T value)
    {
        Value = value;
    }

    public StackItem(T value, StackItem<T> previous)
    {
        Value = value;
        Previous = previous;
    }
}

public class LinkedStack<T>
{
    public StackItem<T> Last { get; set; }
    public int Count { get; set; }

    private LinkedStack(StackItem<T> last, int count)
    {
        Last = last;
        Count = count;
    }

    public LinkedStack()
    {

    }

    public void Push(StackItem<T> item)
    {
        Last = new StackItem<T>(item.Value, Last);
        Count++;
    }

    public StackItem<T> Pop()
    {
        if (Count == 0) throw new Exception("Stack is empty");
        var lastItem = Last;
        Last = Last.Previous;
        Count--;
        return lastItem;
    }

    public void Clear()
    {
        Last = null;
        Count = 0;
    }

    public LinkedStack<T> CreateCopy()
    {
        return new LinkedStack<T>(Last, Count);
    }
}