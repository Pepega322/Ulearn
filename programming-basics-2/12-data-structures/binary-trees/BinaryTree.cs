using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private TreeNode<T> _root;

    public T this[int index]
    {
        get
        {
            if (_root == null) throw new NullReferenceException();
            if (index > _root.NodesCount - 1 || index < 0) throw new IndexOutOfRangeException();
            var node = _root;
            var currentIndex = node.NodesCount - 1 - (node.Right != null ? node.Right.NodesCount : 0);
            while (currentIndex != index)
            {
                if (index < currentIndex)
                {
                    node = node.Left;
                    currentIndex -= 1 + (node.Right != null ? node.Right.NodesCount : 0);
                }
                else
                {
                    node = node.Right;
                    currentIndex += 1 + (node.Left != null ? node.Left.NodesCount : 0);
                }
            }
            return node.Value;
        }
    }

    public void Add(T value)
    {
        if (_root == null)
        {
            _root = new TreeNode<T>(value);
            return;
        }
        var node = _root;
        while (true)
        {
            var nextNode = value.CompareTo(node.Value) < 0 ? node.Left : node.Right;
            if (nextNode is null) break;
            node = nextNode;
        }
        node.Add(value);
    }

    public bool Contains(T value)
    {
        var node = _root;
        while (node != null)
        {
            var compare = value.CompareTo(node.Value);
            if (compare == 0) return true;
            node = compare < 0 ? node.Left : node.Right;
        }
        return false;
    }

    private TreeNode<T> GetMinNodeAtSubtree(TreeNode<T> root)
    {
        if (root is null) return null;
        while (true)
        {
            if (root.Left is null) return root;
            root = root.Left;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_root is null) yield break;
        var stack = new Stack<(TreeNode<T> Min, TreeNode<T> Root)>();
        stack.Push((GetMinNodeAtSubtree(_root), null));
        while (stack.Count != 0)
        {
            var pair = stack.Pop();
            var node = pair.Min;
            yield return node.Value;
            if (node.Parent != pair.Root)
                stack.Push((node.Parent, pair.Root));
            if (node.Right is not null)
                stack.Push((GetMinNodeAtSubtree(node.Right), node));
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TreeNode<T> where T : IComparable
{
    public T Value { get; set; }
    public TreeNode<T> Parent { get; set; }
    public TreeNode<T> Left { get; set; }
    public TreeNode<T> Right { get; set; }
    public int NodesCount { get; private set; }

    public TreeNode(T value)
    {
        Parent = null;
        Value = value;
        NodesCount = 1;
    }

    public void Add(T value)
    {
        var toAdd = new TreeNode<T>(value);
        toAdd.Parent = this;
        IncreaseCount();
        if (value.CompareTo(Value) < 0) Left = toAdd;
        else Right = toAdd;
    }

    private void IncreaseCount()
    {
        NodesCount++;
        if (Parent is not null)
            Parent.IncreaseCount();
    }
}
