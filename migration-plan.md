# .NET 8 Migration

```markdown
- [ ] Update language version
- [ ] Update dotnet version
- [ ] Library upgrades with releated .NET 8
- [ ] Docker upgrade
- [ ] Other library upgrades
- [ ] Syntax improvement
  - [ ] Use primary constructors
    - Parameter name start with underscore
    - Use primary c. where dependency injection and record exist
  - [ ] Use collection expressions
  - [ ] Use default lambda parameters
- [ ] Use/Test source link
- [ ] `DO`: Review DO's exception handling structure and look if you can switch
  to `IExceptionHandling`.
- [ ] `DO`: `TimeProvider` to be injected.
- [ ] `Learn`: Learn how to use `Keyed DI services`
```

## Primary constructors

You can now create primary constructors in any class and struct.

To ensure that all primary constructor parameters are definitely assigned, all
explicitly declared constructors must call the primary constructor using
`this()` syntax.

```csharp
public readonly struct Distance(double dx, double dy)
{
    public readonly double Magnitude { get; } = Math.Sqrt(dx * dx + dy * dy);
    public readonly double Direction { get; } = Math.Atan2(dy, dx);
}
```

### Mutable state

```csharp
public struct Distance(double dx, double dy)
{
    public readonly double Magnitude => Math.Sqrt(dx * dx + dy * dy);
    public readonly double Direction => Math.Atan2(dy, dx);

    public void Translate(double deltaX, double deltaY)
    {
        dx += deltaX;
        dy += deltaY;
    }

    public Distance() : this(0,0) { }
}
```

### DI

```csharp
public interface IService
{
    Distance GetDistance();
}

public class ExampleController(IService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<Distance> Get()
    {
        return service.GetDistance();
    }
}
```

## Collection expressions

Several collection-like types can be created without requiring external BCL
support. These types are:

- Array types, such as `int[]`.
- `System.Span<T>` and `System.ReadOnlySpan<T>`.
- Types that support collection initializers, such as
  `System.Collections.Generic.List<T>`.

```csharp
int[] row0 = [1, 2, 3];
int[] row1 = [4, 5, 6];
int[] row2 = [7, 8, 9];
int[] single = [..row0, ..row1, ..row2];
foreach (var element in single)
{
    Console.Write($"{element}, ");
}
// output:
// 1, 2, 3, 4, 5, 6, 7, 8, 9,
```

## Default lambda parameters

You can now define default values for parameters on lambda expressions.

```csharp
var IncrementBy = (int source, int increment = 1) => source + increment;

Console.WriteLine(IncrementBy(5)); // 6
Console.WriteLine(IncrementBy(5, 2)); // 7
```

## Performance-focused types

The new `System.Collections.Frozen` namespace includes the collection types
`FrozenDictionary<TKey,TValue>` and `FrozenSet<T>`. These types don't allow any
changes to keys and values once a collection created.

The new `System.Buffers.SearchValues<T>` type is designed to be passed to
methods that look for the first occurrence of any value in the passed collection

The new `System.Text.CompositeFormat` type is useful for optimizing format
strings that aren't known at compile time (for example, if the format string is
loaded from a resource file).

## Source Link

Source Link is now included in the .NET SDK. The goal is that by bundling Source
Link into the SDK, instead of requiring a separate `<PackageReference>` for the
package, more packages will include this information by default. That
information will improve the IDE experience for developers.

## Keyed DI services

Keyed dependency injection (DI) services provides a means for registering and
retrieving DI services using keys. By using keys, you can scope how your
register and consume services. These are some of the new APIs:

- The IKeyedServiceProvider interface.
- The ServiceKeyAttribute attribute, which can be used to inject the key that
  was used for registration/resolution in the constructor.
- The FromKeyedServicesAttribute attribute, which can be used on service
  constructor parameters to specify which keyed service to use.
- Various new extension methods for IServiceCollection to support keyed
  services, for example, `ServiceCollectionServiceExtensions.AddKeyedScoped`.
- The ServiceProvider implementation of IKeyedServiceProvider.

```csharp
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BigCacheConsumer>();
builder.Services.AddSingleton<SmallCacheConsumer>();

builder.Services.AddKeyedSingleton<IMemoryCache, BigCache>("big");
builder.Services.AddKeyedSingleton<IMemoryCache, SmallCache>("small");

var app = builder.Build();

app.MapGet("/big", (BigCacheConsumer data) => data.GetData());
app.MapGet("/small", (SmallCacheConsumer data) => data.GetData());

app.Run();

class BigCacheConsumer([FromKeyedServices("big")] IMemoryCache cache)
{
    public object? GetData() => cache.Get("data");
}

class SmallCacheConsumer(IKeyedServiceProvider keyedServiceProvider)
{
    public object? GetData() => keyedServiceProvider.GetRequiredKeyedService<IMemoryCache>("small");
}
```

## `IExceptionHandler`

`IExceptionHandler` is a new interface that gives the developer a callback for
handling known exceptions in a central location.

`IExceptionHandler` implementations are registered by calling
`IServiceCollection.AddExceptionHandler<T>`. Multiple implementations can be
added, and they're called in the order registered.

## Time abstraction

The new TimeProvider class and ITimer interface add time abstraction
functionality, which allows you to mock time in test scenarios.
