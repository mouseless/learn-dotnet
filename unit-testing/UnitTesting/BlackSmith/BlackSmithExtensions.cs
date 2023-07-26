namespace UnitTesting;

public static class BlackSmithExtensions
{
    // GiveMe
    public static Forge AForge(this TestBase.BlackSmith source) => new();

    public static ISword ASword(this TestBase.BlackSmith source, Mold mold, Raw raw) =>
        new Sword(
            mold ?? new() { Shape = Shape.Sword },
            raw ?? new() { Name = "Iron" }
        )
    ;

    public static Mold Mold(this TestBase.BlackSmith source, Shape shape = Shape.Sword) => 
        new Mold() { Shape = shape };

    public static Raw ARaw(this TestBase.BlackSmith source, string name = "Iron") =>
        new Raw() { Name = name };

    // MockMe
    public static IForge AForge(this TestBase.MockSmith source) =>
        new Mock<IForge>().Object;
}
