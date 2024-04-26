namespace NullableUsage;

public class Persons : IFinder
{
    internal static readonly List<Person> List = new();

    public void Add(Person person) => List.Add(person);
    public void Remove(Person person) => List.Remove(person);

    public List<Person> All() => List;
    public Person? Find(string name) => List.FirstOrDefault(p => p.Name == name);
}