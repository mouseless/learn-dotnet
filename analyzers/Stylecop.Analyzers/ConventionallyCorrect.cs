namespace Stylecop.Analyzers;

public class ConventionallyCorrect
{
    readonly string _field;

    public ConventionallyCorrect(string field)
    {
        _field = field;
    }

    public int MyProperty { get; set; }

    public void PublicVoidMethod() { }

    public string PublicNonVoidMethod()
    {
        return "PublicNonVoidMethod";
    }

    public string PublicMethodWithExpressionBody() => "PublicMethodWithExpressionBody";

    string PrivateMethod()
    {
        return "PrivateMethod";
    }
}
