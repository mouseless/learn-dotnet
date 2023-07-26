namespace UnitTesting;

public abstract class TestBase
{
    public BlackSmith GiveMe { get; private set; } = default!;
    public MockSmith MockMe { get; private set; } = default!;

    [SetUp]
    public virtual void SetUp()
    {
        GiveMe = new BlackSmith();
        MockMe = new MockSmith();
    }

    public sealed record BlackSmith();
    public sealed record MockSmith();
}
