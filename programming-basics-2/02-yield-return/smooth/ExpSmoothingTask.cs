using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        DataPoint prevPoint = null;
        foreach (var point in data)
        {
            if (prevPoint is null)
            {
                prevPoint = point.WithExpSmoothedY(point.OriginalY);
                yield return prevPoint;
            }
            else
            {
                var sY = prevPoint.ExpSmoothedY + alpha * (point.OriginalY - prevPoint.ExpSmoothedY);
                var newPoint = point.WithExpSmoothedY(sY);
                yield return newPoint;
                prevPoint = newPoint;
            }
        }
    }
}
