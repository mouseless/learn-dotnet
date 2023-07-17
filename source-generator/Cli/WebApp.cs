namespace Cli;

public class WebApp : IProject
{
    public void Do(string input, string output)
    {
        var jsonFile = File.ReadAllText(input);

        File.WriteAllText(output, jsonFile);
    }
}