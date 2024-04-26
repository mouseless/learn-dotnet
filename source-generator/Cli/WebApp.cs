namespace Cli;

public class WebApp : ICommand
{
    public void Execute(string input, string output)
    {
        var jsonFile = File.ReadAllText(input);

        File.WriteAllText(output, jsonFile);
    }
}