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

You also need to call `UseExceptionHandler` to add the
`ExceptionHandlerMiddleware` to the request pipeline:

```csharp
app.UseExceptionHandler();
```

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
