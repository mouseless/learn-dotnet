using System.ComponentModel.DataAnnotations.Schema;

namespace NullableUsage;

public class Person
{
    [NotMapped]
    readonly Persons _context = default!;

    protected Person() { }
    public Person(Persons context)
    {
        _context = context;
    }

    public string Name { get; private set; } = default!;
    public string? MiddleName { get; private set; }
    public string? InitialName => Name.Length > MiddleName?.Length ? Name : MiddleName;

    public Person With(string name)
    {
        Name = name;

        _context.Add(this);

        return this;
    }

    public Person With(string name, string? middleName)
    {
        Name = name;
        MiddleName = middleName;

        _context.Add(this);

        return this;
    }

    public void Delete()
    {
        _context.Remove(this);
    }

    public void ChangeMiddleName(string middleName)
    {
        MiddleName = middleName;
    }
}
