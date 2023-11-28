using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tickets {
    public class TicketsTask {
        static void Main() {
            var input = Console.ReadLine()
                .Split(' ')
                .Select(e => int.Parse(e))
                .ToArray();
            var result = Solve(input[0], input[1]);
            Console.WriteLine(result);
        }

        public static BigInteger Solve(int halfLen, int totalSum) {
            if (totalSum % 2 == 1) return 0;
            var table = GenerateTable(halfLen, totalSum / 2);
            var num = table[totalSum / 2, halfLen - 1];
            return num * num;
        }

        public static BigInteger[,] GenerateTable(int length, int maxSum) {
            var table = new BigInteger[maxSum + 1, length];
            for (var row = 0; row <= maxSum && row <= 9; row++)
                table[row, 0] = (BigInteger)1;

            for (var col = 1; col < length; col++) {
                var window = new LimetedSizeWindow(10);
                for (var row = 0; row <= maxSum; row++) {
                    window.Add(table[row, col - 1]);
                    table[row, col] = window.Sum;
                }
            }

            return table;
        }

        public class LimetedSizeWindow {
            private Queue<BigInteger> queue = new Queue<BigInteger>();
            public readonly int MaxSize;
            public int Count => queue.Count;
            public BigInteger Sum { get; private set; }

            public LimetedSizeWindow(int size) {
                MaxSize = size;
            }

            public void Add(BigInteger num) {
                queue.Enqueue(num);
                Sum += num;
                if (queue.Count > MaxSize)
                    Sum -= queue.Dequeue();
            }
        }
    }
}
