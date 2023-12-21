namespace Cli;

public class CommandFactory
{
    readonly string _commandName;

    public CommandFactory(string commandName)
    {
        _commandName = commandName;
    }

    public ICommand Create()
    {
        switch (_commandName.ToLower())
        {
            case "domain":
                return new Domain();
            case "webapp":
                return new WebApp();
            default:
                throw new("Not match any class");
        }
    }
}