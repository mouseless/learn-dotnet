namespace Cli;

public class CommandFactory(string _commandName)
{
    public ICommand Create()
    {
        return _commandName.ToLower() switch
        {
            "domain" => new Domain(),
            "webapp" => new WebApp(),
            _ => throw new("Not match any class"),
        };
    }
}