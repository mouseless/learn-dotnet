namespace PrimaryConstructor;

public class Dependent(Dependency _dependency)
{
    public void ShowMessage() => Console.WriteLine(_dependency.Message);

    public void ShowExceptionMessage()
    {
        try
        {
            throw new RecordNotFoundException(typeof(Dependency));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}