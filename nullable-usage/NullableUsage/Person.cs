using System.ComponentModel.DataAnnotations.Schema;

namespace NullableUsage;


public class Person
{
    [NotMapped]
    readonly IEntityContext<Person> _context = default!;

    protected Person() { }
    public Person(IEntityContext<Person> context)
    {
        _context = context;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = default!;
    public string? MiddleName { get; private set; }
    public int? Age { get; set; }
    public string? InitialName => Name.Length > MiddleName?.Length ? Name : MiddleName;

    public Person With(string name)
    {
        Name = name;

        return _context.Insert(this);
    }

    public Person With(string name, string? middleName)
    {
        Name = name;
        MiddleName = middleName;

        return _context.Insert(this);
    }

    public void Delete()
    {
        _context.Delete(this);
    }

    public void ChangeMiddleName(string middleName)
    {
        MiddleName = middleName;
    }
}
