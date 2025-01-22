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

## Authentication and authorization

### OpenIdConnectHandler adds support for Pushed Authorization Requests (PAR)

Pushed Authorization Requests (PAR) is a relatively new OAuth standard that
improves the security of `OAuth` and `OIDC` flows by moving authorization
parameters from the front channel to the back channel. Thats is, moving
authorization parameters from redirect URLs in the browser to direct machine to
machine http calls on the back end.

The identity provider's discovery document is usually found at
`.well-known/openid-configuration`. If this causes problems, you can disable PAR
via `OpenIdConnectOptions.PushedAuthorizationBehavior` as follows:

```csharp
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect("oidc", oidcOptions =>
    {
        // Other provider-specific configuration goes here.

        // The default value is PushedAuthorizationBehavior.UseIfAvailable.

        // 'OpenIdConnectOptions' does not contain a definition for 'PushedAuthorizationBehavior'
        // and no accessible extension method 'PushedAuthorizationBehavior' accepting a first argument
        // of type 'OpenIdConnectOptions' could be found
        oidcOptions.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;
    });
```

### OIDC and OAuth Parameter Customization

```csharp
//old
builder.Services.AddAuthentication().AddOpenIdConnect(options =>
{
    options.Events.OnRedirectToIdentityProvider = context =>
    {
        context.ProtocolMessage.SetParameter("prompt", "login");
        context.ProtocolMessage.SetParameter("audience", "https://api.example.com");
        return Task.CompletedTask;
    };
});
//new
builder.Services.AddAuthentication().AddOpenIdConnect(options =>
{
    options.AdditionalAuthorizationParameters.Add("prompt", "login");
    options.AdditionalAuthorizationParameters.Add("audience", "https://api.example.com");
});
```

## New HybridCache library

The HybridCache API bridges some gaps in the existing `IDistributedCache` and
`IMemoryCache` APIs. It also adds new capabilities, such as:

- "Stampede" protection to prevent parallel fetches of the same work.
- Configurable serialization.

```csharp
//before
public class SomeService(IDistributedCache cache)
{
    public async Task<SomeInformation> GetSomeInformationAsync
        (string name, int id, CancellationToken token = default)
    {
        var key = $"someinfo:{name}:{id}"; // Unique key for this combination.
        var bytes = await cache.GetAsync(key, token); // Try to get from cache.
        SomeInformation info;
        if (bytes is null)
        {
            // Cache miss; get the data from the real source.
            info = await SomeExpensiveOperationAsync(name, id, token);

            // Serialize and cache it.
            bytes = SomeSerializer.Serialize(info);
            await cache.SetAsync(key, bytes, token);
        }
        else
        {
            // Cache hit; deserialize it.
            info = SomeSerializer.Deserialize<SomeInformation>(bytes);
        }
        return info;
    }

    // This is the work we're trying to cache.
    private async Task<SomeInformation> SomeExpensiveOperationAsync(string name, int id,
        CancellationToken token = default)
    { /* ... */ }
}

//after
public class SomeService(HybridCache cache)
{
    public async Task<SomeInformation> GetSomeInformationAsync
        (string name, int id, CancellationToken token = default)
    {
        return await cache.GetOrCreateAsync(
            $"someinfo:{name}:{id}", // Unique key for this combination.
            async cancel => await SomeExpensiveOperationAsync(name, id, cancel),
            token: token
        );
    }
}
```

HybridCache uses the configured `IDistributedCache` implementation, if any, for
secondary out-of-process caching, for example, using Redis. But even without an
`IDistributedCache`, the HybridCache service will still provide in-process
caching and "stampede" protection.

> :information_box:
>
> stampede: Prevents the same data from being received by multiple threads in
> parallel.

#### A note on object reuse

Because a lot of HybridCache usage will be adapted from existing
`IDistributedCache` code, HybridCache preserves this behavior by default to
avoid introducing concurrency bugs. However, a given use case is inherently
thread-safe:

- If the types being cached are immutable.
- If the code doesn't modify them.

## Developer exception page improvements

![](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-9/_static/aspnetcore-developer-page-improvements.gif?view=aspnetcore-9.0)

## ASP0026: Analyzer to warn when `[Authorize]` is overridden by
`[AllowAnonymous]` from "farther away"

In .NET 9 Preview 6, we've introduced an analyzer that will highlight instances
like these where a closer `[Authorize]` attribute gets overridden by an
`[AllowAnonymous]` attribute that is farther away from an MVC action. The
warning points to the overridden `[Authorize]` attribute with the that message.

## Improved Kestrel connection metrics

We've made a significant improvement to Kestrel's connection metrics by
including metadata about why a connection failed. The
`kestrel.connection.duration` metric now includes the connection close reason in
the error.type attribute.

sample of the error.type values:

- `tls_handshake_failed` - The connection requires TLS, and the TLS handshake
  failed.
- `connection_reset` - The connection was unexpectedly closed by the client
  while requests were in progress.
- `request_headers_timeout` - Kestrel closed the connection because it didn't
  receive request headers in time.
- `max_request_body_size_exceeded` - Kestrel closed the connection because
  uploaded data exceeded max size.

## Customize Kestrel named pipe endpoints

Kestrel's named pipe support has been improved with advanced customization
options. The new CreateNamedPipeServerStream method on the named pipe options
allows pipes to be customized per-endpoint.

```csharp
var builder = WebApplication.CreateBuilder();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenNamedPipe("pipe1");
    options.ListenNamedPipe("pipe2");
});

builder.WebHost.UseNamedPipes(options =>
{
    options.CreateNamedPipeServerStream = (context) =>
    {
        var pipeSecurity = CreatePipeSecurity(context.NamedPipeEndpoint.PipeName);

        return NamedPipeServerStreamAcl.Create(context.NamedPipeEndPoint.PipeName, PipeDirection.InOut,
            NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte,
            context.PipeOptions, inBufferSize: 0, outBufferSize: 0, pipeSecurity);
    };
});
```

## `ExceptionHandlerMiddleware` option to choose the status code based on the exception type

A new option when configuring the `ExceptionHandlerMiddleware` enables app
developers to choose what status code to return when an exception occurs during
request handling. The new option changes the status code being set in the
`ProblemDetails` response from the `ExceptionHandlerMiddleware`.

```csharp
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex is TimeoutException
        ? StatusCodes.Status503ServiceUnavailable
        : StatusCodes.Status500InternalServerError,
});
```

## Opt-out of HTTP metrics on certain endpoints and requests

.NET 9 introduces the ability to opt-out of HTTP metrics for specific endpoints
and requests. Performance up.

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/healthz").DisableHttpMetrics();
app.Run();

//or

app.Use(async (context, next) =>
{
    var metricsFeature = context.Features.Get<IHttpMetricsTagsFeature>();
    if (metricsFeature != null &&
        context.Request.Headers.ContainsKey("x-disable-metrics"))
    {
        metricsFeature.MetricsDisabled = true;
    }

    await next(context);
});
```

## Data Protection support for deleting keys

> :warning:
>
> I guess it's none of our business. It seems to be compatible with environments
> like Azure and redis. [See](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/default-settings?view=aspnetcore-9.0)

Prior to .NET 9, data protection keys were not deletable by design, to prevent
data loss. Deleting a key renders its protected data irretrievable.

```csharp
using Microsoft.AspNetCore.DataProtection.KeyManagement;

var services = new ServiceCollection();
services.AddDataProtection();

var serviceProvider = services.BuildServiceProvider();

var keyManager = serviceProvider.GetService<IKeyManager>();

if (keyManager is IDeletableKeyManager deletableKeyManager)
{
    var utcNow = DateTimeOffset.UtcNow;
    var yearAgo = utcNow.AddYears(-1);

    if (!deletableKeyManager.DeleteKeys(key => key.ExpirationDate < yearAgo))
    {
        Console.WriteLine("Failed to delete keys.");
    }
    else
    {
        Console.WriteLine("Old keys deleted successfully.");
    }
}
else
{
    Console.WriteLine("Key manager does not support deletion.");
}
```

## Middleware supports Keyed DI

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddKeyedSingleton<MySingletonClass>("test");
builder.Services.AddKeyedScoped<MyScopedClass>("test2");

var app = builder.Build();
app.UseMiddleware<MyMiddleware>();
app.Run();

internal class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next,
        [FromKeyedServices("test")] MySingletonClass service)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context,
        [FromKeyedServices("test2")]
            MyScopedClass scopedService) => _next(context);
}
```

## Trust the ASP.NET Core HTTPS development certificate on Linux

On Ubuntu and Fedora based Linux distros, `dotnet dev-certs https --trust` now
configures ASP.NET Core HTTPS development certificate as a trusted certificate
for:

- Chromium browsers, for example, Google Chrome, Microsoft Edge, and Chromium.
- Mozilla Firefox and Mozilla derived browsers.
- .NET APIs, for example, HttpClient

Previously, `--trust` only worked on Windows and macOS. Certificate trust is
applied per-user.

## Templates updated to latest Bootstrap, jQuery, and jQuery Validation versions

The ASP.NET Core project templates and libraries have been updated to use the
latest versions of Bootstrap, jQuery, and jQuery Validation, specifically:

- Bootstrap 5.3.3
- jQuery 3.7.1
- jQuery Validation 1.21.0
