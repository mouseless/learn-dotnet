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

    [HttpGet]
    [Route("now")]
    public DateTime GetNow([FromServices] Singleton singleton)
    {
        return singleton.GetNow();
    }

    [HttpGet]
    [Route("employees/manager")]
    public IEmployee GetManager([FromKeyedServices("manager")] IEmployee employee)
    {
        return employee;
    }

    [HttpGet]
    [Route("employees/engineer")]
    public IEmployee GetEngineer([FromKeyedServices("engineer")] IEmployee employee)
    {
        return employee;
    }

    [HttpGet]
    [Route("employees")]
    public IEnumerable<IEmployee> GetEmployees([FromServices] IEnumerable<IEmployee> employees)
    {
        return employees;
    }

    [HttpPost]
    [Route("/transients/dispose")]
    public async Task UseTransientDisposable([FromServices] Func<TransientDisposable> newTransientDisposable)
    {
        using var transientDisposable = newTransientDisposable();

        await transientDisposable.Process();
    }
}