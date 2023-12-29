namespace PrimaryConstructor;

public class Dependent(Dependency _dependency)
{
    public void ShowMessage() => Console.WriteLine(_dependency.Message);

    public void ThrowException() =>
        throw new CustomException("This is CustomException's message");
}