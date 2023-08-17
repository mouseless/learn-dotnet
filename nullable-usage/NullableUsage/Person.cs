namespace NullableUsage;


public class Person
{
    public int Id { get; set; }
    public string Name { get; private set; } = default!;
    public string? MiddleName { get; private set; }
    public int? Age { get; set; }

    public Person With(string name, string? middleName)
    {
        Name = name;
        MiddleName = middleName;

        return this;
    }
}
