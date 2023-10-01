using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        static int[] StartOrder;
        static double BestLength;
        static int[] BestOrder;

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            StartOrder = MakeTrivialPermutation(checkpoints.Length);
            BestLength = double.MaxValue;
            FindBestOrder(checkpoints, new int[StartOrder.Length]);
            var result = BestOrder;
            return result;
        }

        private static void FindBestOrder(Point[] checkpoints, int[] order, int position = 1)
        {
            if (position == order.Length)
            {
                BestLength = checkpoints.GetPathLength(order);
                BestOrder = order.ToArray();
                return;
            }

            for (var i = 1; i < order.Length; i++)
            {
                var wasUsed = (Array.IndexOf(order, StartOrder[i], 0, position) != -1);
                if (!wasUsed)
                {
                    order[position] = StartOrder[i];
                    var currentLength = GetPathLength(checkpoints, order, position);
                    if (currentLength - BestLength > 1e-6) break;
                    else FindBestOrder(checkpoints, order, position + 1);
                }
            }
        }

        private static double GetPathLength(Point[] checkpoints, int[] order, int position)
        {
            double length = 0;
            for (var i = 1; i <= position; i++)
            {
                length += PointExtensions.DistanceTo(
                    checkpoints[order[i - 1]],
                    checkpoints[order[i]]);
            }
            return length;
        }

        private static int[] MakeTrivialPermutation(int size)
        {
            var bestOrder = new int[size];
            for (int i = 0; i < bestOrder.Length; i++)
                bestOrder[i] = i;
            return bestOrder;
        }
    }
}