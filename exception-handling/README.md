# Exception Handling With `IExceptionHandler`

We use `ExceptionHandlers` to manage exceptions in one place, which we derive
from `IExceptionHandler`.

To handle exceptions, you must write an exception class derived from
`IExceptionHandler`.

```csharp
public class CustomExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ...
    }
}
```

> :information_source:
>
> If the exception can be handled, it should return `true`. If the exception
> can't be handled, it should return `false`.

Then you should register it

```csharp
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
```

You also need to call `UseExceptionHandler` which needs options parameters to
add the `ExceptionHandlerMiddleware` to the request pipeline:

```csharp
app.UseExceptionHandler("/error-handling-path");

or

app.UseExceptionHandler(opt => { });
```

But if you add `ProblemDetails` as a service you don't need to give parameter to
`UseExceptionHandler`.

```csharp
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

...

app.UseExceptionHandler();
```

> :information_source:
>
> Default HTTP Status code return is 500, you customize it as follow if you need
>
> ```csharp
> httpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
> ```

## UseExceptionHandler

`UseExceptionHandler` add the `ExceptionHandlerMiddleware` to the request
pipeline.

`ExceptionHandlerMiddleware` catches and logs unhandled exceptions, if a path
is given re-executes the request using the given path. This middleware is to
showcase exception info when the [Developer exception
page](#developer-exception-page) is disabled due to the app not running on the
development environment.

## Problem Details

`Problem Details` is [RFC](https://datatracker.ietf.org/doc/html/rfc7807)
standard to handle errors returned on HTTP APIs responses.

```json
{
    "Type": "",
    "Title": "",
    "Status": 500,
    "Detail": "",
    "Instance": ""
}
```

Adds a `ProblemDetail` model for unhandled exceptions. Exception details are
included in the response body using this default model.

```csharp
builder.Services.AddProblemDetails();
```

When you want to return exception result in the same format in your handle
exceptions, you can give your object of type `ProblemDetails` to the response in
json format with `WriteAsJsonAsync()`.

```csharp
public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
{
    var problemDetails = new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Custom Exception Handler",
        Detail = exception.Message
    };

    httpContext.Response.StatusCode = problemDetails.Status.Value;

    await httpContext.Response
        .WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
}
```

Below you can see the result on the page

```json
{"title":"Custom Exception Handler","status":500,"detail":"Hello Exception"}
```

## Developer Exception Page

You can use

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
```

for better development experience. This will give you more detail for exception.

`UserDeveloperExceptionPage` is enabled by default when the app is running in
the development environment and the app is created using
`WebHost.CreateBuilder`.

## Chaining Exception Handlers

You can add multiple exception handler, and they're called in the order they are
registered. A possible use case for this is using exceptions for flow control.

```csharp
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
```

In this case, in order not to handle the wrong exception, flow control can be
provided by returning `false` of `TryHandleAsync` with an if control for the
type of exceptions you do not want.

```csharp
public class NotFoundExceptionHandler : IExceptionHandler
{
    ...
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }
        ...
    }
}
```
