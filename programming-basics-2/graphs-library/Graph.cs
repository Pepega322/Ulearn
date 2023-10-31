namespace Graphs;

public class Graph {
    public Node[] nodes;

    public Graph(int nodesCount) {
        nodes = Enumerable
        .Range(0, nodesCount)
            .Select(z => new Node(z))
        .ToArray();
    }

    public Node this[int node] => nodes[node];

    public Edge this[int from, int to] => Edges
        .Where(e => e.From.Number == from && e.To.Number == to)
        .Single();

    public int Length { get => nodes.Length; }

    public IEnumerable<Edge> Edges {
        get => nodes
                .SelectMany(n => n.IncidentEdges)
                .Distinct();
    }

    public IEnumerable<Node> Nodes {
        get => nodes
                .Select(n => n);
    }

    public void Connect(int v1, int v2) => nodes[v1].Connect(nodes[v2]);

    public void Delete(Edge edge) {
        foreach (var node in nodes.Where(n => n.IncidentEdges.Contains(edge)))
            node.Disconnect(edge);
    }

    public static Graph MakeGraph(params int[] incidentNodes) {
        var graph = new Graph(incidentNodes.Max() + 1);
        for (var i = 0; i < incidentNodes.Length - 1; i += 2)
            graph.Connect(incidentNodes[i], incidentNodes[i + 1]);
        return graph;
    }
}



