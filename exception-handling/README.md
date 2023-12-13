# Exception Handling

We use `ExceptionHandlers` to manage exceptions in one place, which we derive
from `IExceptionHandler`.

```csharp
public class CustomExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ...

        return true;
    }
}
```

To use it, we register it with `AddExceptionHandler`

```csharp
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
```

> :warning:
>
> When registering multiple exception handlers you should pay attention to the
> order. It works according to the order of insertion.

Also need to call `UseExceptionHandler` to add the `ExceptionHandlerMiddleware`
to the request pipeline

```csharp
app.UseExceptionHandler();
```

We also use `UseExceptionHandler` in development mode because we expect a json
object as a response because we use Postman.

We expect a json object but we don't want a json object in a different format
for each exception, so we use the
[RFC](https://datatracker.ietf.org/doc/html/rfc7807) standard [Problem
Detail](https://datatracker.ietf.org/doc/html/rfc7807#section-3.1)
object to standardize it.

To apply this standard in the exceptions we handle, we create the
`ProblemDetail` object and give it to the response as follows

```csharp
public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
{
    var problemDetails = new ProblemDetails
    {
        ...
    };

    httpContext.Response.StatusCode = problemDetails.Status.Value;

    await httpContext.Response
        .WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
}
```

We fill the content of the object as below.

- **Type**: Exception document link
- **Title**: Exception name without the suffix 'exception' in standard case
- **Status**: Status codes
- **Detail**: Exception's message,
- **Extensions**: Exception's extra fields, if any

Below you can see the example response body on the page

```json
{
    "type":"http://localhost:5032/errors/parameter-required",
    "title":"Parameter Required",
    "status":500,
    "detail":"param2 is required.",
    "name":"param2"
}
```

In order to have the same standard in the exceptions we do not handle, we have
applied the same standard in all exceptions by using `AddProblemDetails` as
below.

```csharp
builder.Services.AddProblemDetails();
```

The only problem now is that exceptions like `404` don't have pages. For such
exceptions, we use `UseStatusCodePages`. In this way, for example, instead of
the browser's warning such as the page could not be found when you go to a path
that does not exist, it shows our page and we display the exception with the
`ProblemDetail` object.

The addition is as follows

```csharp
app.UseStatusCodePages();
```

## Resources

- [Handle errors in ASP.NET Core][handle-errors]
- [Global Error Handling in ASP.NET Core 8][global-error-handling]
- [Handling Web API Exceptions with ProblemDetails middleware][blog-post]

[blog-post]: https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/
[global-error-handling]: https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8#new-way-iexceptionhandler
[handle-errors]: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#server-exception-handling
