namespace PrimaryConstructor;

public class Dependent(Dependency _dependency)
{
    public string ShowMessage() => _dependency.Message;
}