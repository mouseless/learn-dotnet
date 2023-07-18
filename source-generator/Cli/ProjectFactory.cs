namespace Cli;

public class CommandFactory
{
    readonly string _projectName;

    public CommandFactory(string projectName)
    {
        _projectName = projectName;
    }

    public ICommand Create()
    {
        string projectName = _projectName.ToLower();

        switch (_projectName.ToLower())
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