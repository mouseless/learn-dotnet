using Microsoft.AspNetCore.Mvc;

namespace NullableUsage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        readonly PersonService _personService;

        public PersonsController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _personService.All();
        }

        public record AddPerson(string Name, string? MiddleName);

        [HttpPost]
        public Person Add([FromBody] AddPerson input)
        {
            return _personService.AddPerson(input.Name, input.MiddleName);
        }

        [HttpDelete]
        public void Delete([FromQuery] int id)
        {
            _personService.DeletePerson(id);
        }
    }
}