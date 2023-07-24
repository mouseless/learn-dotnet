namespace UnitTesting;

public class Testbase
{
    protected Repository _context;

    [SetUp]
    public void SetUp()
    {
        _context = new Repository();
    }

    protected Person APerson(string name = "") => new Person();
}
