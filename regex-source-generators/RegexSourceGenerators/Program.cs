using RegexSourceGenerators;

Console.WriteLine("Give me an url: ");

string giveMeUrl = Console.ReadLine() ?? string.Empty;

Console.WriteLine(
    giveMeUrl.IsUrl()
    ? "You entered a correct URL"
    : "This is not a url"
);