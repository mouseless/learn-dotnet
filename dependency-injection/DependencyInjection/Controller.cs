using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller
{
    readonly Singleton _singleton;

    public Controller(Singleton singleton) =>
        _singleton = singleton;

    [HttpPost]
    [Route("stuff")]
    public void DoStuff()
    {
        _singleton.DoStuff("controller");
    }
}
