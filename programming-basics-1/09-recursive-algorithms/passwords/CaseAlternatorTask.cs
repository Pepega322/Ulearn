namespace Passwords;

public class CaseAlternatorTask {
    public static List<string> AlternateCharCases(string word) {
        var result = new List<string>();
        AlternateCharCases(word.ToLower().ToCharArray(), 0, result); ;
        return result;
    }

    private static void AlternateCharCases(char[] word, int index, List<string> result) {
        if (index == word.Length) {
            result.Add(string.Join(string.Empty, word));
            return;
        }

        var s = word[index];
        var haveUpperCase = !char.Equals(char.ToLower(s), char.ToUpper(s));
        AlternateCharCases(word, index + 1, result);
        if (char.IsLetter(s) && haveUpperCase) {
            word[index] = char.ToUpper(s);
            AlternateCharCases(word, index + 1, result);
            word[index] = char.ToLower(s);
        }
    }
}
