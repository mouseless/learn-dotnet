# Dependency Injection

We use Microsoft's default service container for dependency injection.

## Singleton Factory for Transient and Scoped

When registering a transient or scoped service `T`, make sure you register a
singleton `Func<T>` that is able to initialize that service lazily. This will
allow you to inject a generic factory that will cause a better readability for
services that require a service provider.

Assume there is a singleton service called `ServiceA`, and a transient, or
scoped, service called `ServiceB`, where `ServiceA` requires an instance of
`ServiceB` when exposing a business functionality;

```csharp
public class ServiceA(IServiceProvider _serviceProvider)
{
    public void DoStuff()
    {
        var serviceB = _serviceProvider.GetRequiredService<ServiceB>();

        serviceB.DoOtherStuff();
    }
}
```

In the above example `ServiceA` has an implicit dependency on `ServiceB`. This
type of usage is called service locator anti-pattern, which we workaround by
using a generic factory as shown in below;

```csharp
public class ServiceA(Func<ServiceB> _newServiceB)
{
    public void DoStuff()
    {
        var serviceB = _newServiceB();

        serviceB.DoStuff();
    }
}
```

In this example, `ServiceA` depends on `ServiceB` explicitly, thus having a
better readability.

> :warning:
>
> When registering a generic factory function, make sure you use
> `HttpContext.RequestServices` instead of root service provider. Otherwise
> scoped dependencies will cause a runtime error.

## Keyed DI Services

Keyed Services allows us to register these services with a key for the service
implementations registered with the same service type for our ease of use and to
access them with this key when calling them.

In the list below you can see extensions that you can use when
registering services with a key.

- `AddKeyedTransient<TService, TImplementation>(object key)`
- `AddKeyedScoped<TService, TImplementation>(object key)`
- `AddKeyedSingleton<TService, TImplementation>(object key)`

As an example, let's assume that we have an `IPersonal` interface and two
`PersonalA` and `PersonalB` services derived from it and register them as
singletons. Let's use an enum to make it easy to recognize.

```csharp
enum ServiceImplementation
{
    PersonalA,
    PersonalB
}

builder.Services.AddKeyedSingleton<IPersonal, PersonalA>(ServiceImplementation.PersonalA);
builder.Services.AddKeyedSingleton<IPersonal, PersonalB>(ServiceImplementation.PersonalB);
```

Now that we have registered them, we need to figure out how to call these
services. We can use many ways for this. Some of the suggested ways:

- `FromKeyedServices` attribute
- `GetRequiredKeyedService` extension of `IServiceProvider`

With the above case in mind, below are examples of the use of these pathways.

```csharp
public void FromKeyedServicesUsing([FromKeyedServices(ServiceImplementation.PersonalA)] IPersonal personal)
{
    ...
}

public void GetRequiredKeyedServiceUsing()
{
    var personal = _serviceProvider.GetRequiredKeyedService<IPersonal>(ServiceImplementation.PersonalB);
    ...
}
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
>   ...
> }
> ```

## Resources

- Keyed DI Services
  - [Keyed Services in .NET8's Dependency Injection][keyed-services-net8-di]
  - [Keyed service dependency injection container support][keyed-services-di-container-support]

[keyed-services-net8-di]: https://dev.to/xaberue/keyed-services-in-net8s-dependency-injection-2gni
[keyed-services-di-container-support]: https://andrewlock.net/exploring-the-dotnet-8-preview-keyed-services-dependency-injection-support/
