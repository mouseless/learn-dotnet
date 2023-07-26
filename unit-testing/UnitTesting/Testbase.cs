namespace UnitTesting;

public abstract class Testbase
{
    public BlackSmith GiveMe { get; private set; } = default!;

    [SetUp]
    public virtual void SetUp()
    {
        GiveMe = new BlackSmith();
    }

    public sealed record BlackSmith();
}
