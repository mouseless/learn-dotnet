namespace NullableUsage;

public class PersonService
{
    readonly Func<Person> _newPerson;
    readonly IQueryContext<Person> _queryContext;

    public PersonService(Func<Person> newPerson, IQueryContext<Person> queryContext)
    {
        _newPerson = newPerson;
        _queryContext = queryContext;
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
        int id,
        string? middleName = default
    )
    {
        if (middleName is null) { throw new ArgumentNullException(); }

        var person = _queryContext.SingleById(id);

        if (person is not null)
        {
            person.ChangeMiddleName(middleName);
        }
    }

    public void DeletePerson(int id)
    {
        _queryContext.SingleById(id)?.Delete();
    }

    public IEnumerable<Person> All() => _queryContext.All();
    public Person? SingleById(int id) => _queryContext.SingleById(id);
}
