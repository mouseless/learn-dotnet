using System.Text.RegularExpressions;

namespace RegexSourceGenerators;

public static partial class LearnRegex
{
    [GeneratedRegex(@"^(https?|ftp)://[^\\s/$.?#].[^\\s]*$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex RegexIsUrl();

    public static bool IsUrl(this string url) => RegexIsUrl().IsMatch(url);
}