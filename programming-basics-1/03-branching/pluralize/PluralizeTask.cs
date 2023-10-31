namespace Pluralize {
    public static class PluralizeTask {
        public static string PluralizeRubles(int count) {
            var lastDigits = ReturnLastNumbers(count);
            if (lastDigits >= 5 || lastDigits == 0) return "рублей";
            if (lastDigits >= 2) return "рубля";
            return "рубль";
        }

        private static int ReturnLastNumbers(int count) {
            if (count % 100 >= 11 && count % 100 <= 14) return 5;
            return count % 10;
        }
    }
}