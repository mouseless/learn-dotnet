namespace UnitTesting;

public static class AssertionExtensions
{
    // Assertion
    public static void ShouldBeValidTrLicensePlateCode(this int source)
    {
        source.ShouldBeGreaterThan(0);
        source.ShouldBeLessThan(82);
    }

    // GiveMe
    public static List<int> Figures(this Spec.Stubber _, int min = 0, int max = 9)
    {
        List<int> result = [];

        for (int figure = min; figure <= max; figure++)
        {
            result.Add(figure);
        }

        return result;
    }
}