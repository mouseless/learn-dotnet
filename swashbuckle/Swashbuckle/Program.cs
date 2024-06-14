using Microsoft.OpenApi.Models;
using Swashbuckle;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(swaggerGenOptions =>
{
    // operation filter
    swaggerGenOptions.OperationFilter<AddParameterToPostOperations>(ParameterLocation.Header, "X-Request-ID");

    // custom operation groups
    swaggerGenOptions.TagActionsBy(api => [api.GroupName]);

    // multi doc
    swaggerGenOptions.SwaggerDoc("admin", new() { Title = "Admin", Version = "v1" });
    swaggerGenOptions.SwaggerDoc("api", new() { Title = "Api", Version = "v1" });

    // custom metadata
    swaggerGenOptions.DocInclusionPredicate((document, api) =>
        // doc exclusion
        !api.CustomAttributes().OfType<InternalAttribute>().Any() &&

        // multi doc
        api.CustomAttributes().OfType<DocumentAttribute>().SingleOrDefault()?.Name == document
      );

    // security config
    swaggerGenOptions.DocumentFilter<DocumentBasedSecurityDefinition>("admin", "AdminKey",
        new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = "X-Admin-Key",
            Description = $"Enter your admin key",
        }
    );
    swaggerGenOptions.DocumentFilter<DocumentBasedSecurityDefinition>("api", "ApiKey",
        new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = "X-API-Key",
            Description = $"Enter your api key",
        }
    );
    swaggerGenOptions.OperationFilter<DocumentBasedSecurityRequirement>("admin", "AdminKey");
    swaggerGenOptions.OperationFilter<DocumentBasedSecurityRequirement>("api", "ApiKey");
});

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    // multi doc
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