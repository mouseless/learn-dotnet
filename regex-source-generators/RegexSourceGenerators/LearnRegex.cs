using System.Text.RegularExpressions;

namespace RegexSourceGenerators;

public partial class LearnRegex
{
    [GeneratedRegex("target", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex RegexOnlyLetter();

    public void GetMeLetters(string text)
    {
        if (RegexOnlyLetter().IsMatch(text))
        {
            Console.WriteLine("IsMatch");
        }
        else
        {
            Console.WriteLine("Is Not Match");
        }
    }
}
