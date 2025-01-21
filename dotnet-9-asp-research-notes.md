# ASP.NET 9 Research Notes

## Static asset delivery optimization

```csharp
+app.MapStaticAssets();
-app.UseStaticFiles();
```

## Minimal APIs

### New `InternalServerError` and `InternalServerError<TValue>` to TypedResults

TypedResults now includes factory methods and types for returning "500 Internal Server Error" responses from endpoints. Here's an example that returns a 500 response:

```csharp
var app = WebApplication.Create();

app.MapGet("/", () => TypedResults.InternalServerError("Something went wrong!"));

app.Run();
```

### Call `ProducesProblem` and `ProducesValidationProblem` on route groups

### `Problem` and `ValidationProblem` result types support construction with `IEnumerable<KeyValuePair<string, object?>>` values

Prior to .NET 9, constructing Problem and ValidationProblem result types in
minimal APIs required that the errors and extensions properties be initialized
with an implementation of `IDictionary<string, object?>`. In this release, these
construction APIs support overloads that consume
`IEnumerable<KeyValuePair<string, object?>>`.

```csharp
var app = WebApplication.Create();

app.MapGet("/", () =>
{
    var extensions = new List<KeyValuePair<string, object?>> { new("test", "value") };
    return TypedResults.Problem("This is an error with extensions",
                                                       extensions: extensions);
});
```

## OpenAPI

### Built-in support for OpenAPI document generation

In .NET 9, ASP.NET Core provides built-in support for generating OpenAPI
documents representing controller-based or minimal APIs via the
`Microsoft.AspNetCore.OpenApi` package.

```csharp
var builder = WebApplication.CreateBuilder();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapGet("/hello/{name}", (string name) => $"Hello {name}"!);

app.Run();
```

OpenAPI documents can also be generated at build-time by adding the
`Microsoft.Extensions.ApiDescription.Server` package.

To modify the location of the emitted OpenAPI documents, set the target path in
the `OpenApiDocumentsDirectory` property in the app's project file:

```xml
<PropertyGroup>
  <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
</PropertyGroup>
```
