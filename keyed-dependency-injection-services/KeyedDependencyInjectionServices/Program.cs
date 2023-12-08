using KeyedDependencyInjectionServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<ICustomService, MyServiceA>("keyServiceA");
builder.Services.AddKeyedSingleton<ICustomService, MyServiceB>("keyServiceB");

var app = builder.Build();

app.MapGet(
    "/serviceA",
    ([FromKeyedServices("keyServiceA")] ICustomService service) => service.ServiceMessage()
);
app.MapGet(
    "/serviceB",
    ([FromKeyedServices("keyServiceB")] ICustomService service) => service.ServiceMessage()
);

app.Run();
