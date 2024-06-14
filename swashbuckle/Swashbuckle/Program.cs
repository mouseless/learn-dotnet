using Microsoft.OpenApi.Models;
using Swashbuckle;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("admin", new() { Title = "admin", Version = "v1" });
    swaggerGenOptions.SwaggerDoc("api", new() { Title = "api", Version = "v1" });
    swaggerGenOptions.OperationFilter<AddParameterToPostOperations>(ParameterLocation.Header, "X-Request-ID");
    swaggerGenOptions.DocInclusionPredicate((document, api) =>
        !api.CustomAttributes().OfType<InternalAttribute>().Any() &&
        api.CustomAttributes().OfType<DocumentAttribute>().SingleOrDefault()?.Name == document
      );
    swaggerGenOptions.TagActionsBy(api => [api.GroupName]);
});

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.SwaggerEndpoint("api/swagger.json", "api");
    swaggerUIOptions.SwaggerEndpoint("admin/swagger.json", "admin");
});
app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");

    return Task.CompletedTask;
});

app.Run();