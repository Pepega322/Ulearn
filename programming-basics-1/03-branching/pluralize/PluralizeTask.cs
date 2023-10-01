namespace Pluralize
{
    public static class PluralizeTask
    {
        public static string PluralizeRubles(int count)
        {
            if (ReturnLastNumbers(count) >= 5 || ReturnLastNumbers(count) == 0) return "рублей";
            else if (ReturnLastNumbers(count) >= 2) return "рубля";
            else return "рубль";
        }

        static int ReturnLastNumbers(int number)
        {
            string numberInString = number.ToString();
            int numberLength = numberInString.Length;

            if (numberLength == 1)
            {
                return number;
            }

            int isItException = int.Parse(numberInString.Substring(numberLength - 2));
            if (isItException >= 11 && isItException <= 14)
            {
                return 5;
            }
            else
            {
                return int.Parse(numberInString.Substring(numberLength - 1));
            }
        }
    }
}