# Keyed Dependency Injection Services

We register Keyed Dependency Injection Services by using `AddKeyedXXX(key)` as
below with its key in order to be able to call the services that we register of
the same type more easily.

```csharp
builder.Services.AddKeyedSingleton<ICustomService, MyServiceA>("keyServiceA");
builder.Services.AddKeyedSingleton<ICustomService, MyServiceB>("keyServiceB");
```

And using `[FromKeyedServices($"{key}")]`, we can call the service we want with
the key we provide as below.

```csharp
app.MapGet("/route", ([FromKeyedServices("keyServiceB")] ICustomService service) => ... )
```

> :information_source:
>
> If you intentionally register more than one service with the same key, you can
> call them all using `IEnumerable` when calling the service, otherwise the last
> one you added will come up.
>
> ```csharp
> public class MyClass([FromKeyedServices("keyService")] IEnumerable<ICustomService> services)
> {
> }
> ```
