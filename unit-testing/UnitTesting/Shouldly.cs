using Shouldly;

namespace UnitTesting;

public class Shouldly : TestBase
{
    [Test]
    public void When_text_is_clipped__Remove_the_spaces_before_and_after()
    {
        string actual = " Test Text ";
        string expected = "Test Text";

        actual.Trim().ShouldBe(expected);
    }

    [Test]
    public void When_forge_is_Given_mold_and_raw_Than_a_sword_is_created()
    {
        // arrange
        var Forge = GiveMe.AForge();
        var raw = GiveMe.ARaw("Iron");
        var expected = GiveMe.ASword(raw);

        // act
        ISword sword = Forge.MakeSword(raw: raw);

        // assert
        sword.ShouldBe(GiveMe.ASword(raw: raw));
    }

    [Test]
    public void If_no_resources_are_given_while_crafting_a_sword__An_exception_is_thrown()
    {
        // arrange
        var forge = GiveMe.AForge();

        // act
        Action act = () => forge.MakeSword(null);

        // assert
        act.ShouldThrow<ArgumentNullException>();
    }
}
