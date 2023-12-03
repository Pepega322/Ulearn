
using System.Data.SqlTypes;

namespace Graphs;

public static class GraphExtensions {
    public static IEnumerable<IEnumerable<Node>> FindConnectedComponents(this Graph graph) {
        var marked = new HashSet<Node>();
        while (true) {
            var node = graph.Nodes.Where(n => !marked.Contains(n)).FirstOrDefault();
            if (node == default) break;
            var breadthSearch = node.BreadthSearch().ToList();
            yield return breadthSearch;
            foreach (var visitedNode in breadthSearch)
                marked.Add(visitedNode);
        }
    }

    public static List<Node>? FindShortestPath(this Graph graph, Node start, Node end) {
        var track = new Dictionary<Node, Node>();
        var queue = new Queue<Node>();
        track[start] = null;
        queue.Enqueue(start);
        while (queue.Count != 0) {
            var node = queue.Dequeue();
            foreach (var nextNode in node.IncidentNodes.Where(n => !track.ContainsKey(n))) {
                track[nextNode] = node;
                queue.Enqueue(nextNode);
            }
            if (track.ContainsKey(end)) break;
        }

        if (!track.ContainsKey(end)) return null;
        var path = new List<Node>();
        while (end != null) {
            path.Add(end);
            end = track[end];
        }
        path.Reverse();
        return path;
    }

    public static List<Node>? KahnAlgorithm(this Graph graph) {
        var topologicSort = new List<Node>();
        var nodes = graph.Nodes.ToList();
        while (nodes.Count != 0) {
            var nodeToDelete = nodes
                .Where(node => !node.IncidentEdges.Any(edge => edge.To == node))
                .FirstOrDefault();
            if (nodeToDelete == null) return null;
            nodes.Remove(nodeToDelete);
            topologicSort.Add(nodeToDelete);
            foreach (var edge in nodeToDelete.IncidentEdges.ToList())
                graph.Delete(edge);
        }
        return topologicSort;
    }

    public static List<Node>? TarjanAlgorithm(this Graph graph) {
        var topSort = new List<Node>();
        var states = graph.Nodes.ToDictionary(node => node, node => State.Unvisited);
        while (true) {
            var whiteNode = states
                .Where(z => z.Value == State.Unvisited)
                .Select(z => z.Key)
                .FirstOrDefault();
            if (whiteNode == default) break;
            if (!TarjanDepthSearch(whiteNode, states, topSort)) return null;
        }
        topSort.Reverse();
        return topSort;
    }

    #region TarjanAlgorithm
    private enum State {
        Unvisited,
        Visited,
        Fineshed
    }

    private static bool TarjanDepthSearch(Node node, Dictionary<Node, State> states, List<Node> topSort) {
        if (states[node] == State.Visited) return false;
        if (states[node] == State.Fineshed) return true;
        states[node] = State.Visited;
        var outgoingNodes = node.IncidentEdges
            .Where(edge => edge.From == node)
            .Select(edge => edge.To);
        foreach (var nextNode in outgoingNodes)
            if (!TarjanDepthSearch(nextNode, states, topSort)) return false;
        states[node] = State.Fineshed;
        topSort.Add(node);
        return true;
    }
    #endregion

    //для неориентированного графа
    public static bool HasCycle(this Graph graph) {
        var nodes = graph.Nodes.ToList();
        var visited = new HashSet<Node>();
        var finished = new HashSet<Node>();
        var stack = new Stack<Node>();
        visited.Add(nodes.First());
        stack.Push(nodes.First());
        while (stack.Count != 0) {
            var node = stack.Pop();
            foreach (var nextNode in node.IncidentNodes.Where(n => !finished.Contains(n))) {
                if (visited.Contains(nextNode)) return true;
                visited.Add(nextNode);
                stack.Push(nextNode);
            }
            finished.Add(node);
        }
        return false;
    }

    public static List<Node>? DijkstraAlgorithm(
        this Graph graph,
        Dictionary<Edge, double> weights,
        Node start,
        Node end,
        IPriorityQueue<Node> queue) {
        var track = new Dictionary<Node, Node>();
        track[start] = null;
        queue.Add(start, 0);

        while (true) {
            var toOpenPair = queue.ExtractMin();
            if (toOpenPair == null) return null;

            var toOpen = toOpenPair.Item1;
            var price = toOpenPair.Item2;
            if (toOpen == end) break;

            foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen)) {
                var next = e.GetOtherNode(toOpen);
                var newPrice = price + weights[e];
                if (queue.UpdateOrAdd(next, newPrice))
                    track[next] = toOpen;
            }
        }

        var result = new List<Node>();
        while (end != null) {
            result.Add(end);
            end = track[end];
        }
        result.Reverse();
        return result;
    }
}

public class DictionaryPriorityQueue<TKey> : IPriorityQueue<TKey>  {
    Dictionary<TKey, double> dictionary = new();

    public void Add(TKey key, double value) {
        dictionary.Add(key, value);
    }

    public void Delete(TKey key) {
        dictionary.Remove(key);
    }

    public Tuple<TKey, double> ExtractMin() {
        if (dictionary.Count == 0) return null;
        var minValue = dictionary.Values.Min();
        var key = dictionary.Where(p => p.Value == minValue).Single().Key;
        dictionary.Remove(key);
        return Tuple.Create(key, minValue);
    }

    public bool TryGetValue(TKey key, out double value) {
        return dictionary.TryGetValue(key, out value);
    }

    public void Update(TKey key, double newValue) {
        dictionary[key] = newValue;
    }
}