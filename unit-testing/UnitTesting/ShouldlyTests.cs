using Shouldly;

namespace UnitTesting;

public class ShouldlyTests : TestBase
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
        var mold = GiveMe.Mold(Shape.Sword);
        var raw = GiveMe.ARaw("Iron");
        var expected = GiveMe.ASword(mold, raw);

        // act
        List<ISword> swords = Forge.MakeSword(mold: mold, raws: new() { raw });

        // assert
        swords.ShouldContain(GiveMe.ASword(mold: mold, raw: raw));
    }
}
