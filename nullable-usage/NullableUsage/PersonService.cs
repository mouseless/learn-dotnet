namespace NullableUsage;

public class PersonService
{
    readonly Func<Person> _newPerson;
    readonly IFinder finder;

    public PersonService(Func<Person> newPerson, IFinder finder)
    {
        _newPerson = newPerson;
        this.finder = finder;
    }

    public Person AddPerson(string? name)
    {
        if (name is null) { throw new ArgumentNullException(); }

        return _newPerson().With(name, null);
    }

    public Person AddPerson(
        string? name = default,
        string? middleName = default
    )
    {
        name ??= "John Doe";

        return _newPerson().With(name, middleName);
    }

    public void UpdatePerson(
        string name,
        string? middleName = default
    )
    {
        if (name is null) { throw new ArgumentNullException(); }
        if (middleName is null) { throw new ArgumentNullException(); }

        var person = finder.Find(name);

        if (person is not null)
        {
            person.ChangeMiddleName(middleName);
        }
    }

    public void DeletePerson(string name)
    {
        finder.Find(name)?.Delete();
    }

    public List<Person> AllPersons() => finder.All();
}
