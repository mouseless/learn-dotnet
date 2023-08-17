namespace NullableUsage;

public class PersonService
{
    readonly Func<Person> _newPerson;
    readonly IEntityContext<Person> _entityContext;
    readonly IQueryContext<Person> _queryContext;

    public PersonService(Func<Person> newPerson, IEntityContext<Person> entityContext, IQueryContext<Person> queryContext)
    {
        _newPerson = newPerson;
        _entityContext = entityContext;
        _queryContext = queryContext;
    }

    public Person AddPerson(string? name)
    {
        if (name is null) throw new ArgumentNullException();

        return _entityContext.Insert(_newPerson().With(name, null));
    }

    public Person AddPerson(
        string? name = default,
        string? middleName = default
    )
    {
        name ??= "John Doe";

        return _entityContext.Insert(_newPerson().With(name, middleName));
    }

    public IEnumerable<Person> All() => _queryContext.All();
    public Person? SingleById(int id) => _queryContext.SingleById(id);
}
