namespace Graphs;

public static class IPriorityQueueExtensions
{
    public static bool UpdateOrAdd<TKey>(this IPriorityQueue<TKey> queue, TKey node, double newPrice)
    {
        double oldPrice;
        var containsNode = queue.TryGetValue(node, out oldPrice);
        if (!containsNode || newPrice < oldPrice)
        {
            queue.Update(node, newPrice);
            return true;
        }
        return false;
    }
}
