# ASP.NET Core 8

## Antiforgery with Minimal APIs

This release adds a middleware for validating antiforgery tokens, which are used
to mitigate cross-site request forgery attacks. Call AddAntiforgery to register
antiforgery services in DI. `WebApplicationBuilder` automatically adds the
middleware when the antiforgery services have been registered in the DI
container. Antiforgery tokens are used to mitigate cross-site request forgery
attacks.

```csharp
var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();
...
```

## New `IResettable` interface in `ObjectPool`

`Microsoft.Extensions.ObjectPool` provides support for pooling object instances
in memory. Apps can use an object pool if the values are expensive to allocate
or initialize.

### ObjectPool

Microsoft.Extensions.ObjectPool is part of the ASP.NET Core infrastructure that
supports keeping a group of objects in memory for reuse rather than allowing the
objects to be garbage collected. All the static and instance methods in
`Microsoft.Extensions.ObjectPool` are thread-safe.

## Native AOT

Support for .NET native ahead-of-time (AOT) has been added. Apps that are
published using AOT can have substantially better performance: smaller app size,
less memory usage, and faster startup time.

## New project template

### New CreateSlimBuilder method

The `CreateSlimBuilder()` method used in the Web API (native AOT) template
initializes the `WebApplicationBuilder` with the minimum ASP.NET Core features
necessary to run an app.

The `CreateSlimBuilder` method includes the following features that are
typically needed for an efficient development experience:

- JSON file configuration for `appsettings.json` and
  `appsettings.{EnvironmentName}.json`.
- User secrets configuration.
- Console logging.
- Logging configuration.

### New `CreateEmptyBuilder` method

There's another new `WebApplicationBuilder` factory method for building small
apps that only contain necessary features:
`WebApplication.CreateEmptyBuilder(WebApplicationOptions options)`. This
`WebApplicationBuilder` is created with no built-in behavior.

```csharp
var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());
builder.WebHost.UseKestrelCore();

var app = builder.Build();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello, World!");
    await next(context);
});

Console.WriteLine("Running...");
app.Run();
```

## Authentication and authorization

ASP.NET Core 8 introduces the `IAuthorizationRequirementData` interface. The
`IAuthorizationRequirementData` interface allows the attribute definition to
specify the requirements associated with the authorization policy.

[Details](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-8.0?view=aspnetcore-8.0#iauthorizationrequirementdata)

## Securing Swagger UI endpoints

Swagger UI endpoints can now be secured in production environments by calling
`MapSwagger().RequireAuthorization`.

## Miscellaneous

### Keyed services support in Dependency Injection

Keyed services refers to a mechanism for registering and retrieving Dependency
Injection (DI) services using keys. A service is associated with a key by
calling AddKeyedSingleton (or AddKeyedScoped or AddKeyedTransient) to register
it. Access a registered service by specifying the key with the
`[FromKeyedServices]` attribute.

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<ICache, BigCache>("big");
builder.Services.AddKeyedSingleton<ICache, SmallCache>("small");
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/big", ([FromKeyedServices("big")] ICache bigCache) => bigCache.Get("date"));
app.MapGet("/small", ([FromKeyedServices("small")] ICache smallCache) =>
                                                               smallCache.Get("date"));

app.MapControllers();

app.Run();

public interface ICache
{
    object Get(string key);
}
public class BigCache : ICache
{
    public object Get(string key) => $"Resolving {key} from big cache.";
}

public class SmallCache : ICache
{
    public object Get(string key) => $"Resolving {key} from small cache.";
}

[ApiController]
[Route("/cache")]
public class CustomServicesApiController : Controller
{
    [HttpGet("big-cache")]
    public ActionResult<object> GetOk([FromKeyedServices("big")] ICache cache)
    {
        return cache.Get("data-mvc");
    }
}

public class MyHub : Hub
{
    public void Method([FromKeyedServices("small")] ICache cache)
    {
        Console.WriteLine(cache.Get("signalr"));
    }
}
```

### Visual Studio project templates for SPA apps with ASP.NET Core backend

Visual Studio project templates are now the recommended way to create
single-page apps (SPAs) that have an ASP.NET Core backend. Templates are
provided that create apps based on the JavaScript frameworks Angular, React, and
Vue. These templates:

- Create a Visual Studio solution with a frontend project and a backend project.
- Use the Visual Studio project type for JavaScript and TypeScript (.esproj) for
  the frontend.
- Use an ASP.NET Core project for the backend.

### Support for generic attributes

Attributes that previously required a Type parameter are now available in
cleaner generic variants. This is made possible by support for generic
attributes in C# 11. For example, the syntax for annotating the response type of
an action can be modified as follows:

```csharp
[ApiController]
[Route("api/[controller]")]
public class TodosController : Controller
{
  [HttpGet("/")]
- [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
+ [ProducesResponseType<Todo>(StatusCodes.Status200OK)]
  public Todo Get() => new Todo(1, "Write a sample", DateTime.Now, false);
}
```

Generic variants are supported for the following attributes:

- `[ProducesResponseType<T>]`
- `[Produces<T>]`
- `[MiddlewareFilter<T>]`
- `[ModelBinder<T>]`
- `[ModelMetadataType<T>]`
- `[ServiceFilter<T>]`
- `[TypeFilter<T>]`

### `IExceptionHandler`

`IExceptionHandler` is a new interface that gives the developer a callback for
handling known exceptions in a central location.

`IExceptionHandler` implementations are registered by calling
`IServiceCollection.AddExceptionHandler<T>`. Multiple implementations can be
added, and they're called in the order registered.

### Short-circuit middleware after routing

Use the ShortCircuit extension method to cause routing to invoke the endpoint logic immediately and then end the request. For example, a given route might not need to go through authentication or CORS middleware. The following example short-circuits requests that match the /short-circuit route:

```csharp
app.MapGet("/short-circuit", () => "Short circuiting!").ShortCircuit();
---
app.MapShortCircuit(404, "robots.txt", "favicon.ico");
```

### HTTP logging middleware extensibility

- `HttpLoggingFields.Duration`
- `HttpLoggingOptions.CombineLogs`
- `IHttpLoggingInterceptor`

### New APIs in ProblemDetails to support more resilient integrations

In .NET 8, a new API was added to make it easier to implement fallback behavior
if `IProblemDetailsService` isn't able to generate `ProblemDetails`.
