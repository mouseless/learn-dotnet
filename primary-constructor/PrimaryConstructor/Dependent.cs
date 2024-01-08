namespace PrimaryConstructor;

public class Dependent(Dependency _dependency)
    : DependentBase(_dependency.Message)
{
    public void ShowMessage() => Console.WriteLine(_dependency.Message);

    public void ThrowException() =>
        throw new CustomException("This is CustomException's message");
}