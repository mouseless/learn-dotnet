namespace UnitTesting;

public class Repository
{
    List<Person> _persons;

    public Repository()
    {
        _persons = new List<Person>();
    }

    public List<Person> Persons { get; set; }

    public void Save()
    {
        Persons = _persons;
    }

    public void Add(Person person)
    {
        _persons.Add(person);
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}