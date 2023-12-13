using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller(Singleton _singleton, IServiceProvider _serviceProvider)
{
    [HttpPost]
    [Route("stuff")]
    public void DoStuff()
    {
        _singleton.DoStuff("controller");
    }

    [HttpPost]
    [Route("from-keyed-services/personal-a")]
    public string FromKeyedServicesUsing([FromKeyedServices("personal-a")] IPersonal personal)
    {
        return personal.Name;
    }

    [HttpPost]
    [Route("get-required-keyed-service-using/personal-b")]
    public string GetRequiredKeyedServiceUsing()
    {
        var personal = _serviceProvider.GetRequiredKeyedService<IPersonal>("personal-b");

        return personal.Name;
    }
}
