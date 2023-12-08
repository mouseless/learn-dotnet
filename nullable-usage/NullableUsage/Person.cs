namespace NullableUsage;

public class Person(Persons _context)
{
    protected Person() : this(default!) { }

    public string Name { get; private set; } = default!;
    public string? MiddleName { get; private set; }
    public string? InitialName => Name.Length > MiddleName?.Length ? Name : MiddleName;

    public Person With(string name, string? middleName = default)
    {
        Name = name;
        MiddleName = middleName;

        _context.Add(this);

        return this;
    }

    public void ChangeMiddleName(string middleName) => MiddleName = middleName;

    public void Delete() => _context.Remove(this);
}
