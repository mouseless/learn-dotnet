using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapGet("/", () => "go to /summing?param1={0}&param2={1} route for summing");
app.MapGet("/summing", ([FromQuery] int? param1, [FromQuery] int? param2) =>
{
    if (param1 is null) { throw new ParameterRequiredException(nameof(param1)); }
    if (param2 is null) { throw new ParameterRequiredException(nameof(param2)); }

    return param1 + param2;
});
app.MapGet("/errors/parameter-required", () => "This exception occurs when a required parameter is missing.");

app.Run();
