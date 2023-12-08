using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller(Singleton _singleton)
{
    [HttpPost]
    [Route("stuff")]
    public void DoStuff()
    {
        _singleton.DoStuff("controller");
    }
}
