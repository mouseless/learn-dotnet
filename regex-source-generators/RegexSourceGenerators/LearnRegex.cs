using System.Text.RegularExpressions;

namespace RegexSourceGenerators;

public partial class LearnRegex
{
    [GeneratedRegex("abc|def", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex RegexOnlyLetter();

    public static void GetMeLetters(string text)
    {
        if (RegexOnlyLetter().IsMatch(text))
        {
            // Take action with matching text
        }
    }
}
