# net 8

## Serialization

### Interface hierarchies

.NET 8 adds support for serializing properties from interface hierarchies.

```csharp
IDerived value = new DerivedImplement { Base = 0, Derived = 1 };
JsonSerializer.Serialize(value);
// {"Base":0,"Derived":1}

public interface IBase
{
    public int Base { get; set; }
}

public interface IDerived : IBase
{
    public int Derived { get; set; }
}

public class DerivedImplement : IDerived
{
    public int Base { get; set; }
    public int Derived { get; set; }
}
```

### Naming policies

`JsonNamingPolicy` includes new naming policies for `snake_case`
(with an underscore) and `kebab-case` (with a hyphen) property name conversions.

```csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
};
JsonSerializer.Serialize(new { PropertyName = "value" }, options);
// { "property_name" : "value" }
```

### Read-only properties

You can now deserialize onto read-only fields or properties (that is, those that
don't have a set accessor).

## Time abstraction

The new TimeProvider class and ITimer interface add time abstraction
functionality, which allows you to mock time in test scenarios.

## UTF8 improvements

If you want to enable writing out a string-like representation of your type to a
destination span, implement the new `IUtf8SpanFormattable` interface on your
type. This new interface is closely related to `ISpanFormattable`, but targets
`UTF8` and `Span<byte>` instead of `UTF16` and `Span<char>`.

## Methods for working with randomness

The new `System.Random.GetItems` and
`System.Security.Cryptography.RandomNumberGenerator.GetItems` methods let you
randomly choose a specified number of items from an input set.

- `Shuffle<T>()` randomize the order of a span.
- `GetItems<T>()` randomly choose a specified number of items from an input set.

## Performance-focused types

The new `System.Collections.Frozen` namespace includes the collection types
`FrozenDictionary<TKey,TValue>` and `FrozenSet<T>`. These types don't allow any
changes to keys and values once a collection created.

The new `System.Buffers.SearchValues<T>` type is designed to be passed to
methods that look for the first occurrence of any value in the passed collection

The new `System.Text.CompositeFormat` type is useful for optimizing format
strings that aren't known at compile time (for example, if the format string is
loaded from a resource file).

New `System.IO.Hashing.XxHash3` and `System.IO.Hashing.XxHash128` types provide
implementations of the fast `XXH3` and `XXH128` hash algorithms

## Data validation

`System.ComponentModel.DataAnnotations` namespace includes new data validation
attributes intended for validation scenarios in cloud-native services.

- `RangeAttribute.MinimumIsExclusive`
- `RangeAttribute.MaximumIsExclusive`
- `System.ComponentModel.DataAnnotations.LengthAttribute`
- `System.ComponentModel.DataAnnotations.Base64StringAttribute`
- `System.ComponentModel.DataAnnotations.AllowedValuesAttribute`
- `System.ComponentModel.DataAnnotations.DeniedValuesAttribute`

## Cryptography

.NET 8 adds support for the SHA-3 hashing primitives.

## Networking

`HttpClient` now supports `HTTPS` proxy, which creates an encrypted channel
between the client and the proxy so all requests can be handled with full
privacy.

## Extension libraries

### Keyed DI services

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

### Hosted lifecycle services

Hosted services now have more options for execution during the application
lifecycle. `IHostedService` provided `StartAsync` and `StopAsync`, and now
`IHostedLifecycleService` provides these additional methods:

- `StartingAsync(CancellationToken)`
- `StartedAsync(CancellationToken)`
- `StoppingAsync(CancellationToken)`
- `StoppedAsync(CancellationToken)`

## Garbage collection

`.NET 8` adds a capability to adjust the memory limit on the fly. This is useful
in cloud-service scenarios, where demand comes and goes. To be cost-effective,
services should scale up and down on resource consumption as the demand
fluctuates. When a service detects a decrease in demand, it can scale down
resource consumption by reducing its memory limit.

```csharp
GC.RefreshMemoryLimit();
```

```csharp
AppContext.SetData("GCHeapHardLimit", (ulong)100 * 1_024 * 1_024);
GC.RefreshMemoryLimit();
```

The API can throw an `InvalidOperationException` if the hard limit is invalid,
for example, in the case of negative heap hard limit percentages and if the hard
limit is too low.

## Configuration-binding source generator

.NET 8 introduces a source generator to provide AOT and trim-friendly
configuration in ASP.NET Core. The generator is an alternative to the
pre-existing reflection-based implementation.

The source generator probes for Configure(TOptions), Bind, and Get calls to
retrieve type info from. When the generator is enabled in a project, the
compiler implicitly chooses generated methods over the pre-existing
reflection-based framework implementations.

No source code changes are needed to use the generator. It's enabled by default
in AOT'd web apps. For other project types, the source generator is off by
default, but you can opt in by setting the `EnableConfigurationBindingGenerator`
property to true in your project file

## Reflection improvements

Starting in .NET 8, a System.Type object is returned instead. This type provides
access to function pointer metadata, including the calling conventions, return
type, and parameters.

```csharp
// Sample class that contains a function pointer field.
public unsafe class UClass
{
    public delegate* unmanaged[Cdecl, SuppressGCTransition]<in int, void> _fp;
}

// ...

FieldInfo fieldInfo = typeof(UClass).GetField(nameof(UClass._fp));

// Obtain the function pointer type from a field.
Type fpType = fieldInfo.FieldType;

// New methods to determine if a type is a function pointer.
Console.WriteLine(
    $"IsFunctionPointer: {fpType.IsFunctionPointer}");
Console.WriteLine(
    $"IsUnmanagedFunctionPointer: {fpType.IsUnmanagedFunctionPointer}");

// New methods to obtain the return and parameter types.
Console.WriteLine($"Return type: {fpType.GetFunctionPointerReturnType()}");
```

```bash
IsFunctionPointer: True
IsUnmanagedFunctionPointer: True
Return type: System.Void
```

## Native AOT support

.NET 8 brings the following improvements to Native AOT publishing:

- Adds support for the x64 and Arm64 architectures on macOS.

- Reduces the sizes of Native AOT apps on Linux by up to 50%. The following
  table shows the size of a "Hello World" app published with Native AOT that
  includes the entire .NET runtime on .NET 7 vs.

- Lets you specify an optimization preference: size or speed. By default, the
  compiler chooses to generate fast code while being mindful of the size of the
  application. However, you can use the `<OptimizationPreference>` MSBuild
  property to optimize specifically for one or the other. For more information,
  see Optimize AOT deployments.

## Terminal build output

`dotnet build` has a new option to produce more modernized build output. This
terminal logger output groups errors with the project they came from, better
differentiates the different target frameworks for multi-targeted projects, and
provides real-time information about what the build is doing. To opt into the
new output, use the `--tl` option.

## Simplified output paths

.NET 8 introduces an option to simplify the output path and folder structure for
build outputs. Previously, .NET apps produced a deep and complex set of output
paths for different build artifacts. The new, simplified output path structure
gathers all build outputs into a common location, which makes it easier for
tooling to anticipate.

## `dotnet publish` and `dotnet pack` assets

Since the dotnet publish and dotnet pack commands are intended to produce
production assets, they now produce Release assets by default.

The following output shows the different behavior between `dotnet build` and
`dotnet publish`, and how you can revert to publishing Debug assets by setting
the `PublishRelease` property to `false`.

## `dotnet restore` security auditing

When you run dotnet add or dotnet restore, warnings NU1901-NU1904 will appear
for any vulnerabilities that are found. For more information, see Audit for
[security vulnerabilities](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-restore#audit-for-security-vulnerabilities)

## Source Link

Source Link is now included in the .NET SDK. The goal is that by bundling Source
Link into the SDK, instead of requiring a separate `<PackageReference>` for the
package, more packages will include this information by default. That
information will improve the IDE experience for developers.

Eklenen paketlerde source link otomatik olarak enable geliyor ve debug yaparken
gidemediğimiz kaynak koda artık gidebiliyoruz. Ama paket in bunu desteklemesi
gerek.

## Build your own .NET on Linux

In .NET 8, that's no longer necessary and you can build .NET on Linux directly
from the dotnet/dotnet repository. That repo uses dotnet/source-build to build
.NET runtimes, tools, and SDKs.

Building in a container is the easiest approach for most people, since the
`dotnet-buildtools/prereqs` container images contain all the required
dependencies. For more information, see the
[build instructions](https://github.com/dotnet/dotnet#building).

## Cross-built Windows apps

When you build apps that target Windows on non-Windows platforms, the resulting
executable is now updated with any specified Win32 resources—for example,
application icon, manifest, version information.

## NuGet

Starting in .NET 8, NuGet verifies signed packages on Linux by default. NuGet
continues to verify signed packages on Windows as well.

## Activity operation name when null

```csharp
new Activity(operationName: null).OperationName // Value is "".
```

## Breaking Changes

- Default ASP.NET Core port changed to 8080
- `Base64.DecodeFromUtf8(ReadOnlySpan<Byte>, Span<Byte>, Int32, Int32, Boolean)`
  and `Base64.DecodeFromUtf8InPlace(Span<Byte>, Int32)` now ignore whitespace
  (specifically ' ', '\t', '\r', and '\n') in the input, which matches the
  behavior of `Convert.FromBase64String(String)`.
- Starting in .NET 8, an `InvalidOperationException` is thrown if you try to
  format, parse, or convert a Boolean-backed enumeration type.
- Starting in .NET 8, `ConstructorBuilder` and `MethodBuilder` generate IL for
  method parameters where the `HasDefaultValue` of the parameters is set to
  false, which is the expected value.
- `RSA.EncryptValue` and `RSA.DecryptValue` are obsolete
- ConfigurationBinder throws for mismatched value => if a configuration value
  can't be converted to the type of the value in the model, an
  `InvalidOperationException` is thrown.
- `System.Configuration.ConfigurationManager` package does not reference the
  `System.Security.Permissions` package.
- Empty keys added to dictionary by configuration binder
- Date and time converters honor culture argument
- The SDK doesn't change the terminal encoding after exit for other programs.
  By default, the SDK no longer uses UTF-8 for Windows versions that don't
  support it.
- CLI console output uses UTF-8
- `dotnet pack` uses Release configuration
- `dotnet publish` uses Release configuration
- The global using directive for `System.Net.Http` is no longer added
  automatically.
- `dotnet restore` produces security vulnerability warnings by default for all
  restored projects.
- .NET 8.0.1xx SDK versions require Visual Studio 2022 version 17.7 and MSBuild
  version 17.7.
- unlisted tool versions aren't installed unless you specify the exact version
  using the `--version` option and brackets around the version number. For
  example, `--version [5.0.0]`.
- Trimming may not be used with .NET Standard or .NET Framework
