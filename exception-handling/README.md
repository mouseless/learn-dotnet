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
