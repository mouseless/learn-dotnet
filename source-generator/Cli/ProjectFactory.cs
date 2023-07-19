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
        string commandName = _commandName.ToLower();

        switch (_commandName.ToLower())
        {
            case "domain":
                return new Domain();
            case "webapp":
                return new WebApp();
            default:
                throw new Exception("Not match any class");
        }
    }
}