using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection;

[ApiController]
public class Controller(IServiceProvider _serviceProvider, Singleton _singleton)
{
    [HttpPost]
    [Route("stuff")]
    public void DoStuff()
    {
        _singleton.DoStuff("controller");
    }

    [HttpPost]
    [Route("from-keyed-services/personal-a")]
    public string FromKeyedServicesUsing([FromKeyedServices(ServiceImplementation.PersonalA)] IPersonal personal)
    {
        return personal.Name;
    }

    [HttpPost]
    [Route("get-required-keyed-service-using/personal-b")]
    public string GetRequiredKeyedServiceUsing()
    {
        var personal = _serviceProvider.GetRequiredKeyedService<IPersonal>(ServiceImplementation.PersonalB);

        return personal.Name;
    }
}
