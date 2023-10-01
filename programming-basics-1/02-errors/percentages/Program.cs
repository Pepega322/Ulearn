using System;
using System.Globalization;

namespace Percentages
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userInput = Console.ReadLine();
            Console.WriteLine(Calculate(userInput));
        }

        static double SplitAndParse(string userInput, int number)
        {
            return double.Parse(userInput.Split(' ')[number], CultureInfo.InvariantCulture);
        }
        static double Calculate(string userInput)
        {
            double amountOfMoney = SplitAndParse(userInput, 0);
            double rateInPercents = SplitAndParse(userInput, 1);
            double numberOfMonth = SplitAndParse(userInput, 2);
            double moneyMultiplier = 1+0.01*rateInPercents/12;
            return amountOfMoney * Math.Pow(moneyMultiplier, numberOfMonth);
        }
    }
}
