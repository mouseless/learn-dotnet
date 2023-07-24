using FluentAssertions;

namespace UnitTesting;

public class FluentAssertions
{
    [Test]
    public void When_text_is_clipped__Remove_the_spaces_before_and_after()
    {
        string actual = " Test Text ";
        string expected = "Test Text";

        actual.Trim().Should().Be(expected);
    }
}
