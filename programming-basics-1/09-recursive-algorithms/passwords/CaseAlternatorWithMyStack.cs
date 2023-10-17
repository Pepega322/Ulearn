namespace Passwords;
//как вы уже знаете из курса, у нас есть стек, а есть куча
//и в рекурсии мы используем стек, который ограничен по размерам
//если же реализовать всё как в варианте ниже - мы будем использовать стек,
//который как все другие экземпляры классов лежит в куче
//это может быть полезно, если нужно совершить ооочень большой перебор
//при обычной рекурсии может выскочить StackOverflow,
//даже если вы правильно написали выход из рекурсии

//не забудь преименовать класс
public class CaseAlternatorTaskWithMyStack {
    public static List<string> AlternateCharCases(string word) {
        var result = new List<string>();
        //это нужно чтобы обойти проверку на рекурсию на сайте
        AlternateCharCases(word.ToCharArray(), 0, result);

        var stack = new Stack<(char[] Word, int Index)>();
        stack.Push((word.ToLower().ToCharArray(), 0));
        while (stack.Count != 0) {
            var p = stack.Pop();
            if (p.Index == word.Length) {
                result.Add(string.Join(string.Empty, p.Word));
                continue;
            }

            var s = p.Word[p.Index];
            if (char.IsLetter(s) && !Equals(char.ToLower(s), char.ToUpper(s))) {
                p.Word[p.Index] = char.ToUpper(s);
                stack.Push((p.Word.ToArray(), p.Index + 1));
                p.Word[p.Index] = char.ToLower(s);
            }
            stack.Push((p.Word.ToArray(), p.Index + 1));
        }
        return result;
    }

    private static void AlternateCharCases(char[] word, int startIndex, List<string> result) {
    }
}