namespace Graphs;

public interface IPriorityQueue<TKey>
{
    bool TryGetValue(TKey key, out double value);
    void Add(TKey key, double value);
    void Delete(TKey key);
    void Update(TKey key, double newValue);
    Tuple<TKey, double> ExtractMin();
}
