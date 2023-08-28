namespace Stylecop.Analyzers;

public class Test
{
    private readonly string _name;

    public Test()
    {
    }

    public int MyProperty { get; set; }

    private string TestMethod()
    {
        return "Test";
    }
}
