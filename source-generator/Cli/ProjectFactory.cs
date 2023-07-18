namespace Cli;

public class ProjectFactory
{
    readonly string _projectName;

    public ProjectFactory(string projectName)
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