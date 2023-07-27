namespace UnitTesting
{
    public class Moq : TestBase
    {

        [Test]
        public void If_a_tool_is_given_while_making_a_sword_the_tool_is_used()
        {
            // arrange
            var tool = MockMe.ATool();
            var forge = GiveMe.AForge(tool);
            var raw = GiveMe.ARaw();

            // act
            forge.MakeSword(raw);

            // assert
            tool.VerifyUsedTool();
        }

        [Test]
        public void If_the_raw_material_is_not_given_while_making_a_sword_the_tool_will_not_be_used()
        {
            // arrange
            var tool = MockMe.ATool();
            var forge = GiveMe.AForge(tool);

            // act
            Action action = () => forge.MakeSword(null);

            // assert
            tool.VerifyNotUsedTool(action);
        }
    }
}
