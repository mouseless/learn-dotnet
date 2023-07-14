using System.Text.RegularExpressions;

var process = args[0];
var input = args[1];
var output = args[2];

if(process == "pre")
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
else if (process == "post")
{
    var jsonFile = File.ReadAllText(input);

    File.WriteAllText(output, jsonFile);
}
