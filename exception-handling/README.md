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

And register it

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

When we handle exception, we give the response message to [Problem
Detail](#problem-details) object. Message content is as follows

- **Type**: Exception document link
- **Title**: Exception name without the suffix 'exception' in standard case
- **Status**: Status codes
- **Detail**: Exception's message,
- **Extensions**: Exception's extra fields, if any

Below you can see the example response body on the page

```json
{"type":"http://localhost:5032/errors/parameter-required","title":"Parameter Required","status":500,"detail":"param2 is required."}
```

## Problem Details

`Problem Details` is [RFC](https://datatracker.ietf.org/doc/html/rfc7807)
standard to handle errors returned on HTTP APIs responses. You can look at the
members of the `ProblemDetail` object from this
[link](https://datatracker.ietf.org/doc/html/rfc7807#section-3.1).

To use `ProblemDetail` for all unhandled exception messages we use
`AddProblemDetail()` as below.

```csharp
builder.Services.AddProblemDetails();
```

In the exceptions we handle, we give the `ProblemDetail` object we created to
the response as below.

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

## Developer Exception Page

Since we are requesting from Postman, we do not use developer exception page, we
expect exception in response body in development environment. That's why we
don't want a developer exception page.

## UseStatusCodePages

By default, an ASP.NET Core application does not provide a status code page for
HTTP error status codes such as 404 - Not Found, so we use `UseStatusCodePages`.

When using `AddProblemDetail`, we use `UseStatusCodePages` in its bare form
since `AddProblemDetail` already generates the response body for exceptions.

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
