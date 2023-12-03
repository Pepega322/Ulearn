using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskTree;

public class DiskTreeTask
{
    public static List<string> Solve(List<string> input)
    {
        var tree = new FoldersTree();
        foreach (var path in input)
            tree.AddFoldersByPath(path);
        return tree.ToStrings();
    }
}

public class FoldersTree
{
    private Folder _root = new("ROOT");

    public void AddFoldersByPath(string path)
    {
        var current = _root;
        foreach (var subfolderName in path.Split('\\'))
        {
            current.TryAdd(subfolderName, out Folder subfolder);
            current = subfolder;
        }
    }

    public List<string> ToStrings()
    {
        var result = new List<string>();
        var builder = new StringBuilder();
        var stack = new Stack<(Folder Folder, int Depth)>();
        foreach (var pair in _root.Subfolders.Reverse())
            stack.Push((pair.Value, 0));
        while (stack.Count != 0)
        {
            var pair = stack.Pop();
            builder.Append(new string(' ', pair.Depth));
            builder.Append(pair.Folder.Name);
            result.Add(builder.ToString());
            builder.Clear();
            foreach (var nextPair in pair.Folder.Subfolders.Reverse())
                stack.Push((nextPair.Value, pair.Depth + 1));
        }
        return result;
    }
}

public class Folder
{
    public string Name { get; private set; }
    public SortedDictionary<string, Folder> Subfolders { get; private set; }

    public Folder(string name)
    {
        Name = name;
        Subfolders = new(StringComparer.Ordinal);
    }

    public bool TryAdd(string name, out Folder subfolder)
    {
        if (Subfolders.ContainsKey(name))
        {
            subfolder = Subfolders[name];
            return false;
        }
        subfolder = new Folder(name);
        Subfolders.Add(name, subfolder);
        return true;
    }
}
