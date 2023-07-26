namespace UnitTesting;

public static class BlackSmithExtensions
{
    public static Forge AForge(this Testbase.BlackSmith source) => new();
}
