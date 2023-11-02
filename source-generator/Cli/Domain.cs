using System.Text.RegularExpressions;

namespace Cli;

public class Domain : ICommand
{
    public void Execute(string input, string output)
    {
        var generatedCode = File.ReadAllText(input);

        string pattern = @"===JSON BEGIN===\s*(.*?)\s*===JSON END===";
        Match match = Regex.Match(generatedCode, pattern);

        string capturedText = string.Empty;

        if (match.Success)
        {
            capturedText = match.Groups[1].Value;
        }

        capturedText = capturedText.Replace("'", "\"");

        File.WriteAllText(output, capturedText);
    }
}