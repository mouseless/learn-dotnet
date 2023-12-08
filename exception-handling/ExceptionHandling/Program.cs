using ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.MapGet("/", () => "Hello World!");
app.MapGet("/exception-test", () => { throw new("Hello Exception"); });

app.Run();
