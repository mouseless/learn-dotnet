namespace NullableUsage;

public interface IFinder
{
    List<Person> All();
    Person? Find(string name);
}