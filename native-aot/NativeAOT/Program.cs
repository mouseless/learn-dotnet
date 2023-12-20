using System.Data.Common;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using NativeAOT;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddScoped<Database>();
builder.Services.AddScoped<DbConnection, SqliteConnection>(_ =>
    new SqliteConnection($"Data Source=native.sqlite")
);

builder.Services.AddSingleton<DependService>();
builder.Services.AddSingleton<MyService>();
builder.Services.AddLogging();

var app = builder.Build();

var api = app.MapGroup("/test");
api.MapGet("/di", (MyService _myService) => _myService.MethodStringReturn());
api.MapGet("/type", (MyService _myService) => _myService.MethodType());
api.MapGet("/generic-type", (MyService _myService) => _myService.MethodGenericType());
api.MapGet("/logging", (MyService _myService) => _myService.MethodLogging());
api.MapGet("/access-assembly", (MyService _myService) => _myService.MethodAccessAssembly());
api.MapGet("/todo/{id}", async (int id, Database db) => await db.GetById(id));
api.MapGet("/todo", async (Database db) => await db.GetAll());
api.MapPost("/todo", ([FromBody] Todo todo, Database db) => db.Insert(todo));

app.Run();

[JsonSerializable(typeof(List<Todo>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
