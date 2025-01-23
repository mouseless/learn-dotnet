using System.Text.RegularExpressions;

namespace RegexSourceGenerators;

public static partial class LearnRegex
{
    [GeneratedRegex(@"^(https?|ftp)://[^\\s/$.?#].[^\\s]*$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex RegexIsUrl { get; }

    [GeneratedRegex(@"^(?:https?://)?(?:www\.)?([^:/\?#]+)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex RegexGetDomain();

    public static bool IsUrl(this string url) => RegexIsUrl.IsMatch(url);
    public static string GetDomain(this string url)
    {
        var match = RegexGetDomain().Match(url);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}