using Shouldly;
using System.Dynamic;

namespace UnitTesting;

public class Assertions : Spec
{
    [Test]
    public void Objects_are_equal_assertion()
    {
        double expected = 1.0;

        double actual = Math.Floor(1.5);

        actual.ShouldBe(expected);
    }

    [Test]
    public void Throws_exception_message_assertion()
    {
        Action action = () => throw new Exception("test");

        action.ShouldThrow<Exception>().Message.ShouldBe("test");
    }

    [Test]
    public void Throws_exception_assertion()
    {
        Action action = () => throw new ArgumentNullException();

        action.ShouldThrow<ArgumentNullException>();
    }

    [Test]
    public void Null_check_assertion()
    {
        string? actual = null;

        actual.ShouldBeNull();
    }

    [Test]
    public void Boolean_assertion()
    {
        bool actual = true;

        actual.ShouldBeTrue();
    }

    [Test]
    public void Collection_assertion()
    {
        var figures = new List<int>()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        figures.Count.ShouldBe(10);
        figures.ShouldBeUnique();
        figures.ShouldAllBe(f => f > -1 && f < 10);
        figures.ShouldContain(0);
    }

    [Test]
    public void Dictionary_contains_key_assertion()
    {
        var test = new Dictionary<string, string> { { "key", "value" } };

        test.ShouldContainKey("key");
    }

    [Test]
    public void Dictionary_contains_key_and_value_assertion()
    {
        var test = new Dictionary<string, string> { { "key", "value" } };

        test.ShouldContainKeyAndValue("key", "value");
    }

    [Test]
    public void Dynamic_assertion()
    {
        dynamic theFuture = new ExpandoObject();

        theFuture.test = "test";

        DynamicShould.HaveProperty(theFuture, "test");
    }
}
