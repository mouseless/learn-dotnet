using System.Text.RegularExpressions;

namespace Cli;

public class Domain : IProject
{
    public void Do(string input, string output)
    {
        var generatedCode = File.ReadAllText(input);

        string pattern = @"===JSON BEGIN===\s*(.*?)\s*===JSON END===";
        Match match = Regex.Match(generatedCode, pattern);

        string capturedText = "";

        if (match.Success)
        {
            capturedText = match.Groups[1].Value;
        }

        capturedText = capturedText.Replace("'", "\"");

        File.WriteAllText(output, capturedText);
    }
}