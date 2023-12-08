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
([FromKeyedServices("keyServiceB")] ICustomService service) => ...
```
