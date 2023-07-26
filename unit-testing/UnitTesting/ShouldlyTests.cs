using Shouldly;

namespace UnitTesting;

public class ShouldlyTests
{
    [Test]
    public void When_text_is_clipped__Remove_the_spaces_before_and_after()
    {
        string actual = " Test Text ";
        string expected = "Test Text";

        actual.Trim().ShouldBe(expected);
    }

    [Test]
    public void When_forge_is_Given_length_and_raw_Than_a_sword_is_created()
    {
        // arrange
        var Forge = GiveMe.AForge();

        // act
        Forge.CreateASword(length: 10, raw: "Iron");

        // assert
        Forge.CreatedSwords.ShouldContainKeyAndValue(10, "Iron");
    }
}
