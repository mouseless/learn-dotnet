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
public class ServiceA
{
    readonly IServiceProvider _serviceProvider;

    public ServiceA(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

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
public class ServiceA
{
    readonly Func<ServiceB> _newServiceB;

    public ServiceA(Func<ServiceB> newServiceB) =>
        _newServiceB = newServiceB;

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

## Disposable Services

When using factory method for getting services, we use 
`HttpContext.RequestServices`.

```csharp
builder.Services.AddSingleton<Func<Scoped>>(sp => () =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;

    return httpContext.RequestServices.GetRequiredService<Scoped>()
} );
```

Ifyou want to perform extra operations during disposal of a service, you can 
register a service which implements `IDisposable` and `Dispose()` method
will be called at the end of each request.

```csharp
public class DisposableService : IDisposable
{
    void IDisposable.Dispose()
    {
        // Do Stuff
    }
}
```

### Manually disposing a disposable transient

The below code demonstrates a manually disposing a service with using statment. 
However, since the service is resolved from `HttpContext.RequestServices`, 
dispose will be called a second time when request is completed. 

```csharp
public async Task Process()
{
    using var transientDisposable = _newTransientDisposable();

    await transientDisposable.Process();
}
```

See [TransientDisposable.cs](./DependencyInjection/TransientDisposable.cs) for
a solution to avoid any issues when dispose is called a second time.
