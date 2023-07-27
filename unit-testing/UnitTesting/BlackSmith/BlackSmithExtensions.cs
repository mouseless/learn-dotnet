namespace UnitTesting;

public static class BlackSmithExtensions
{
    // GiveMe
    public static Forge AForge(this TestBase.BlackSmith source) => new(new Bellows());
    public static Forge AForge(this TestBase.BlackSmith source, ITool tool) => new(tool);

    public static ISword ASword(this TestBase.BlackSmith source, Raw raw) => new Sword(raw);

    public static Raw ARaw(this TestBase.BlackSmith source, string name = "Iron") =>
        new Raw() { Name = name };

    // MockMe
    public static IForge AForge(this TestBase.MockSmith source) =>
        new Mock<IForge>().Object;

    public static ITool ATool(this TestBase.MockSmith source)
    {
        var mock = new Mock<ITool>();
        mock.Setup(m => m.Use());

        return mock.Object;
    }

    public static void VerifyUsedTool(this ITool source) =>
        Mock.Get(source).Verify(tool => tool.Use());

    public static void VerifyNotUsedTool(this ITool source, Action action)
    {
        try
        { 
            action(); 
        } 
        catch { }

        Mock.Get(source).Verify(tool => tool.Use(), Times.Never);
    }
}
