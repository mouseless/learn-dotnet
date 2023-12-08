using Microsoft.Extensions.Logging;

namespace NullableUsage;

public class PersonService(Func<Person> _newPerson, IFinder _finder, ILogger<PersonService> _logger, Json _json)
{
    public void AddPerson(string? name)
    {
        if (name is null) { throw new ArgumentNullException(nameof(name)); }

        AddPerson(name, null);
    }

    public void AddPerson(
        string? name = default,
        string? middleName = default
    )
    {
        _logger.LogInformation($"Try adding person => name: {name}, middleName: {middleName}");

        name ??= "John Doe";

        var person = _newPerson().With(name, middleName);

        _logger.LogDebug($"Added person => {_json.SerializeWithFormat(person)}");
    }

    public void UpdatePerson(
        string name,
        string middleName
    )
    {
        _logger.LogInformation($"Try updating person => name: {name}, middleName: {middleName}");

        if (name is null) { throw new ArgumentNullException(nameof(name)); }
        if (middleName is null) { throw new ArgumentNullException(nameof(middleName)); }

        var person = _finder.Find(name);

        if (person is not null)
        {
            person.ChangeMiddleName(middleName);

            _logger.LogDebug($"Updated person => {_json.SerializeWithFormat(person)}");
        }
    }

    public void DeletePerson(string name)
    {
        _logger.LogInformation($"Try deleting person=> name: {name}");

        var person = _finder.Find(name);

        if (person is not null)
        {
            person.Delete();

            _logger.LogDebug($"Deleted person=> {name}");
        }
    }

    public void DisplayPeople()
    {
        _logger.LogInformation($"People=>{_json.SerializeWithFormat(_finder.All())}");
    }
}
