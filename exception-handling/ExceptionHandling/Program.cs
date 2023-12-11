using ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.MapGet("/", () => { throw new("Hello Exception"); });

app.Run();
