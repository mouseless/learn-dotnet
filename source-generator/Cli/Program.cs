using System.Text.RegularExpressions;

var input = args[0];
var output1 = args[1];
var output2 = args[2];

var generatedCode = File.ReadAllText(input);

string pattern = @"===JSON BEGIN===\s*(.*?)\s*===JSON END===";
Match match = Regex.Match(generatedCode, pattern);

string capturedText = "";

if (match.Success)
{
   capturedText = match.Groups[1].Value;
}

capturedText = capturedText.Replace("'", "\"");

//parse json from generated code
File.WriteAllText(output1, capturedText);
File.WriteAllText(output2, capturedText);
