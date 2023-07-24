using FluentAssertions;

namespace UnitTesting;

public class FluentAssertions : Testbase
{
    [Test]
    public void When_text_is_clipped__Remove_the_spaces_before_and_after()
    {
        string actual = " Test Text ";
        string expected = "Test Text";

        actual.Trim().Should().Be(expected);
    }

    [Test]
    public void Triple_A_pattern()
    {
        // arrange
        Person person = APerson(name: "Mark");

        // act
        _context.Add(person);
        _context.Save();

        // assert
        _context.Persons.Should().NotBeEmpty();
        // daha eklenecek
    }
}
