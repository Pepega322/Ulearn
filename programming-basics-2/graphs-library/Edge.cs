namespace Graphs;

public class Edge {
    public readonly Node From;
    public readonly Node To;

    public Edge(Node from, Node to) {
        From = from;
        To = to;
    }

    public override string ToString() => $"{From.Number} - {To.Number}";

    public bool IsIncident(Node node) => node == From || node == To;

    public Node GetOtherNode(Node node) {
        if (!IsIncident(node)) throw new ArgumentException();
        return node == From ? To : From;
    }
}

