using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller
{
    [HttpPost]
    [Route("stuff")]
    public void DoStuff([FromServices] Singleton singleton)
    {
        singleton.DoStuff("controller");
    }

    [HttpPost]
    [Route("employee/manager-name")]
    public string GetManagerName([FromKeyedServices("manager")] IEmployee personal)
    {
        return personal.Name;
    }

    [HttpPost]
    [Route("employee/engineer-name")]
    public string GetEngineerName([FromKeyedServices("engineer")] IEmployee personal)
    {
        return personal.Name;
    }

    [HttpPost]
    [Route("employee/names")]
    public List<string> GetEmployeesName([FromKeyedServices("employee")] IEnumerable<IEmployee> employees)
    {
        return employees.Select(p => p.Name).ToList();
    }
}
