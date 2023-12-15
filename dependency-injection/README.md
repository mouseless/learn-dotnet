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

## In Controllers

When getting the services we register in the controllers, if we register with
key, we get them with `FromKeyedServices` attribute in the relevant action. If
we register normally we prefer to get them with `FromServices` attribute.

```csharp
public void Action([FromKeyedServices("key")] ServiceType service) { }

public void Action([FromServices] ServiceType service) { }
```

> :information_source:
>
> If you intentionally register more than one service with the same key, you can
> call them all using `IEnumerable` when calling the service, otherwise the last
> one you added will come up.

## TimeProvider

Previously, we were spending additional effort to make the parts associated with
the `DateTime` object testable and low dependencies. To reduce this, we started
using `TimeProvider`. We use `TimeProvider` by getting it from the relevant
constructor using dependency injection.

## Resources

- In Controllers
  - [Keyed Services in .NET8's Dependency Injection][keyed-services-net8-di]
  - [Keyed service dependency injection container support][keyed-services-di-container-support]

[keyed-services-net8-di]: https://dev.to/xaberue/keyed-services-in-net8s-dependency-injection-2gni
[keyed-services-di-container-support]: https://andrewlock.net/exploring-the-dotnet-8-preview-keyed-services-dependency-injection-support/
