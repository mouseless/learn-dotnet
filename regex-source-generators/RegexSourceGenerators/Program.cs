using RegexSourceGenerators;

Console.WriteLine("Give me an url: ");

string giveMeUrl = Console.ReadLine() ?? string.Empty;

if (giveMeUrl.IsUrl())
{
    Console.WriteLine("Your url domain is: " + giveMeUrl.GetDomain());
}
else
{
    Console.WriteLine("This is not a url");
}