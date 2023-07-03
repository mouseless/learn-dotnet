var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => GeneratedClass.PrintMessage());

app.Run();
