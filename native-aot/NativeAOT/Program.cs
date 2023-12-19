using NativeAOT;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<DependService>();
builder.Services.AddSingleton<MyService>();
builder.Services.AddLogging();

var app = builder.Build();

var api = app.MapGroup("/test");
api.MapGet("/di", (MyService _myService) => _myService.MethodStringReturn());
api.MapGet("/type", (MyService _myService) => _myService.MethodType());
api.MapGet("/generic-type", (MyService _myService) => _myService.MethodGenericType());
api.MapGet("/logging", (MyService _myService) => _myService.MethodLogging());

app.Run();
