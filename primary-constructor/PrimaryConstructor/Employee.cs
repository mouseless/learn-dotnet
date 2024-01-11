public class Employee(string name, DateTime dateOfHire)
{
    public string Name { get; } = name;
    public DateTime DateOfHire { get; } = dateOfHire;

    public string Seniority =>
        DateTime.Now.Year - DateOfHire.Year > 5
        ? "Senior"
        : "Junior";
}
