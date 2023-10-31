using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var queue = new Queue<DataPoint>();
        var sma = 0.0;
        foreach (var point in data)
        {
            if (queue.Count < windowWidth)
            {
                queue.Enqueue(point);
                sma += (point.OriginalY - sma) / queue.Count;
            }
            else
            {
                var lastPoint = queue.Dequeue();
                queue.Enqueue(point);
                sma += (point.OriginalY - lastPoint.OriginalY) / windowWidth;
            }
            yield return point.WithAvgSmoothedY(sma);
        }
    }
}