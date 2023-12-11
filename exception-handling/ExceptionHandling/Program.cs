using ExceptionHandling;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");

app.MapGet("/", () => { throw new("Hello Exception"); });

app.Run();
