using System;

namespace Percentages {
    public class Program {
        static void Main() {
            var credit = new Credit(Console.ReadLine());
            Console.WriteLine(credit.FinalAmount);
        }
    }

    internal class Credit {
        public readonly double Amount;
        public readonly double InterestRate;
        public readonly double CreditPeriod;

        public double FinalAmount {
            get {
                var multiplier = 1 + InterestRate / 12 / 100;
                return Amount * Math.Pow(multiplier, CreditPeriod);
            }
        }

        public Credit(string input) {
            var data = input.Split(' ');
            Amount = double.Parse(data[0]);
            InterestRate = double.Parse(data[1]);
            CreditPeriod = double.Parse(data[2]);
        }
    }
}
