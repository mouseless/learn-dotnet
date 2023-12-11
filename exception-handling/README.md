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

You also need to call `UseExceptionHandler` with `errorHandlingPath` to add the
`ExceptionHandlerMiddleware` to the request pipeline:

```csharp
app.UseExceptionHandler("/error-handling-path");
```

## UseExceptionHandler

`UseExceptionHandler` add the `ExceptionHandlerMiddleware` to the request
pipeline. `UseExceptionHandler()` verilmezse ExceptionHandler exception'ı handle etmek için çağırır. exception logunu basar ancak `UseExceptionHandler` verilmediği için `TryHandleException` false döner. developer exception page açılır. Unhandled exception atmış olur.

## Problem Details

Exceptionlar için genel bir json modelidir.

```json
{
    "Type": "",
    "Title": "",
    "Status": 500,
    "Detail": "",
    "Instance": "",
    "Extensions": [],
}
```

```csharp
builder.Services.AddProblemDetails();
```

ise Handle edilmeyen exceptionlar için kullanılacak bir default problem detail modeli ekler.
response body'e bu model verilir.

İstenirse overrload'u olan configure parametresiylse configure edilebilir.

## When is Development Mode

You can use

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
```

for better development experience. This will give you more detail for exception.

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
