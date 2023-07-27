using Shouldly;

namespace UnitTesting;

public class Shouldly : TestBase
{
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
        sword.ShouldBe(expected);
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

    [Test]
    public void When_the_sword_is_made_its_raw_will_be_of_the_type_of_raw_given()
    {
        // arrange
        var Forge = GiveMe.AForge();
        var raw = GiveMe.ARaw();

        // act
        Sword sword = (Sword)Forge.MakeSword(raw: raw);

        // assert
        sword.Raw.ShouldBe(raw.Name);
    }
}
