using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller
{
    readonly Singleton _singleton;
    readonly ILogger<Controller> _logger;

    public Controller(Singleton singleton, ILogger<Controller> logger)
    {
        _singleton = singleton;
        _logger = logger;
    }

    [HttpPost]
    [Route("stuff")]
    public void DoStuff()
    {
        _singleton.DoStuff("controller");
    }

    [HttpGet]
    [Route("/transient-disposable-test")]
    public async Task TransientDisposable()
    {
        await _singleton.TestTransientDisposable();
    }

    [HttpGet]
    [Route("/transient-disposable-with-using")]
    public async Task TransientDisposableWithUsing()
    {
        await _singleton.TestTransientDisposableWithUsing();
    }
}
