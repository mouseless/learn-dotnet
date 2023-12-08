# Exception Handling With `IExceptionHandler`

We use `ExceptionHandlers` to manage exceptions in one place, which we derive
from `IExceptionHandler`.

To handle exceptions, you must write an exception class derived from
`IExceptionHandler`.

```csharp
public class CustomExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
    )
    {
        ...
    }
}
```

Then you should register it

```csharp
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
```

> :warning:
>
> To chain your Exception handlers and only want to handle exception with them,
> chain them, but you HAVE TO define a default Exception handler that will run
> (and placed in the last position) to handle any Exception that has been
> handled by the previous handlers.
> **The order matters!**
>
> ```csharp
> builder.Services.AddExceptionHandler<TCustomExceptionHandler>();
> builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
> ```

You also need to call `UseExceptionHandler` to add the
`ExceptionHandlerMiddleware` to the request pipeline:

```csharp
app.UseExceptionHandler();
```
