namespace PrimaryConstructor;

public abstract class DependentBase(string _message)
{
    public void ShowBaseMessage() =>
        Console.WriteLine($"This is base class message: {_message}");
}
