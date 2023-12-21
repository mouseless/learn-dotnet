namespace NativeAOT;

public class SampleClass
{
    private string _message;

    public SampleClass()
    {
        _message = "Hello Reflection";
    }

    public void GetMessage()
    {
        Console.WriteLine(_message);
    }
}
