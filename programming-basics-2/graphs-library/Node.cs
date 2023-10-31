namespace Graphs;

public class Node {
    private readonly List<Edge> incidentEdges = new List<Edge>();
    public readonly int Number;

    public Node(int number) {
        Number = number;
    }

    public override string ToString() => Number.ToString();

    public IEnumerable<Node> IncidentNodes {
        get => incidentEdges.Select(e => e.GetOtherNode(this));
    }

    public IEnumerable<Edge> IncidentEdges {
        get => incidentEdges.Select(e => e);
    }

    public void Connect(Node node) {
        var edge = new Edge(this, node);
        incidentEdges.Add(edge);
        node.incidentEdges.Add(edge);
    }

    public void Disconnect(Edge edge) {
        edge.From.incidentEdges.Remove(edge);
        edge.To.incidentEdges.Remove(edge);
    }
}

