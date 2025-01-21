# .NET 9 Libraries Research Notes

The focus of .Net 9 is on cloud-specific applications and performance,
[see](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview)
. Below, I will list the new features related to `baked` and wep apps.

## System.Text.Json

`baked` is using `Newtonsoft.Json`. So i will ignore this.

## Linq

In `Linq`, the new methods `CountBy` and `AggregateBy` added.

### `CountBy`

[Before]
```csharp
class User
{
    public string Name { get; set; }
    public string Role { get; set; }
}

var users = new List<User>
{
    new User { Name = "Alice", Role = "Admin" },
    new User { Name = "Bob", Role = "Member" },
    new User { Name = "Charlie", Role = "Admin" },
    new User { Name = "David", Role = "Member" },
    new User { Name = "Eve", Role = "Guest" },
    new User { Name = "Frank", Role = "Admin" }
};

var roleCounts = users
    .GroupBy(user => user.Role)
    .Select(group => new { Role = group.Key, Count = group.Count() });
```

[After]
```csharp
var roleCounts = users.CountBy(user => user.Role);
// [(Role, Count), (Role, Count), ....]
```

### `AggregateBy`

[Before]
```csharp
class User
{
    public string Name { get; set; }
    public string Role { get; set; }
    public int AccessLevel { get; set; }
}

var users = new List<User>
{
    new User { Name = "Alice", Role = "Admin", AccessLevel = 10 },
    new User { Name = "Bob", Role = "Member", AccessLevel = 5 },
    new User { Name = "Charlie", Role = "Admin", AccessLevel = 20 },
    new User { Name = "David", Role = "Member", AccessLevel = 5 },
    new User { Name = "Eve", Role = "Guest", AccessLevel = 1 },
    new User { Name = "Frank", Role = "Admin", AccessLevel = 10 }
};

var accessLevelSumByRole = users
    .GroupBy(user => user.Role)
    .Select(group => new { Role = group.Key, TotalAccessLevel = group.Sum(user => user.AccessLevel) });
```

[After]
```csharp
var accessLevelSumByRole = users.AggregateBy(
    user => user.Role,
    seed: 0,
    (currentTotal, user) => currentTotal + user.AccessLevel);
// [(Key, Value), (Key, Value), ...]
```

### `Index<TSource>(IEnumerable<TSource>)`

Makes it possible to quickly extract the implicit index of an enumerable. You
can now write code such as the following snippet to automatically index items in
a collection.

```csharp
IEnumerable<string> lines2 = File.ReadAllLines("output.txt");
foreach ((int index, string line) in lines2.Index())
{
    Console.WriteLine($"Line number: {index + 1}, Line: {line}");
}
```

## `PriorityQueue.Remove()`

New `Remove` method lets you update the priority of an item in the queue

## `Base64Url`

Base64Url has been added because there are problems with special characters
(such as +, /) when Base64 is used for urls.

## `BinaryFormatter` Removed

It is throwing exceptions, although there's still access.

## New Customized Spans

Added spans of new customized `Dictionary<TKey,TValue>` and `HashSet<T>` types.

- `Dictionary<TKey,TValue>.GetAlternateLookup`
- `OrderedDictionary<TKey, TValue>`
- `ReadOnlySet<T>`
- ...

## Component model - TypeDescriptor Trimming Support

Applications such as `NativeAOT` and `WinForm` are trimming due to downsizing.
With the new features (e.g. RegisterType) we can preserve the properties of the
Class.

```csharp
// The Type from typeof() is passed to a different method.
// The trimmer doesn't know about ExampleClass anymore
// and thus there will be warnings when trimming.
Test(typeof(ExampleClass));

static void Test(Type type)
{
    // When publishing self-contained + trimmed,
    // this line produces warnings IL2026 and IL2067.
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);

    // When publishing self-contained + trimmed,
    // the property count is 0 here instead of 2.
    Console.WriteLine($"Property count: {properties.Count}");

    // To avoid the warning and ensure reflection
    // can see the properties, register the type:
    TypeDescriptor.RegisterType<ExampleClass>();
    // Get properties from the registered type.
    properties = TypeDescriptor.GetPropertiesFromRegisteredType(type);

    Console.WriteLine($"Property count: {properties.Count}");
}

public class ExampleClass
{
    public string? Property1 { get; set; }
    public int Property2 { get; set; }
}
```

## Cryptography

### CryptographicOperations.HashData

It provides an API that allows developers to hash data more easily by using the
`HashAlgorithmName` parameter to specify which hashing algorithm to use: this.

### KMAC Algorithm

### X.509 certificate loading

`X509CertificateLoader` was introduced based on the “one method, one purpose”
design in response to long certificate installation codes due to different file
extensions in different OS.

## New `TimeSpan.From*` Overloads

[Example]
```csharp
TimeSpan timeSpan1 = TimeSpan.FromSeconds(value: 101.832);
Console.WriteLine($"timeSpan1 = {timeSpan1}");
// timeSpan1 = 00:01:41.8319999

TimeSpan timeSpan2 = TimeSpan.FromSeconds(seconds: 101, milliseconds: 832);
Console.WriteLine($"timeSpan2 = {timeSpan2}");
```

- `FromDays`
- `FromHours`
- `FromMinutes`
- `FromSeconds`
- `FromMilliseconds`
- `FromMicroseconds`

## Logging source generator

> :warning:
>
> Only works with primary constructor !

```csharp
partial class ClassWithPrimaryConstructor(ILogger logger)
{
    [LoggerMessage(0, LogLevel.Debug, "Test.")]
    public partial void Test();
}
```

## `allows ref struct` Used in Libraries

> `allows ref struct` detailed on `C#13` sections.

Several features of the library use or support `allows ref struct`. An example
of `string.Create` is shown below.

```csharp
public static string Create<TState>(int length, TState state, SpanAction<char, TState> action)
    where TState : allows ref struct;

public static string ToLowerInvariant(ReadOnlySpan<char> input) =>
    string.Create(span.Length, input, static (stringBuffer, input) => span.ToLowerInvariant(stringBuffer));
```

## `SearchValues` expansion

`.NET 8` introduced the `SearchValues<T>` type, which provides an optimized
solution for searching for specific sets of characters or bytes within spans. In
`.NET 9`, `SearchValues` has been extended to support searching for substrings
within a larger string.

```csharp
private static readonly SearchValues<string> s_animals =
    SearchValues.Create(["cat", "mouse", "dog", "dolphin"], StringComparison.OrdinalIgnoreCase);

public static int IndexOfAnimal(string text) =>
    text.AsSpan().IndexOfAny(s_animals);
```

## `SocketsHttpHandler` is default in `HttpClientFactory`

`HttpClientFactory` creates `HttpClient` objects backed by `HttpClientHandler`,
by default. `HttpClientHandler` is itself backed by `SocketsHttpHandler`, which
is much more configurable, including around connection lifetime management.
`HttpClientFactory` now uses SocketsHttpHandler by default and configures it to
set limits on its connection lifetimes to match that of the rotation lifetime
specified in the factory.

## `HttpClientFactory` no longer logs header values by default

## `WebSocket` keep-alive ping and timeout

I knew `WebSocket` was not used but I wanted to make a note.

## Reflection

### Persisted assemblies

`.NET 9` introduces the `PersistedAssemblyBuilder` class, which supports
creating and saving dynamic assemblies. This brings the assembly-saving
capabilities from `.NET Framework` to `.NET`.

- Assembly Saving: Dynamically create types and methods, then save them as
  executable (.exe) or library (.dll) files.
- PDB Support: Add symbol information for debugging.
- Advantage: Simplifies migration from `.NET Framework` to `.NET 9` and
  modernizes dynamic code workflows.
This enables assemblies to be both created and stored for later use.

### Type-name parsing

`TypeName` class for parsing and handling `ECMA-335` type names, similar to
`System.Type` but independent of the runtime. It's designed for tools like
serializers.

```csharp
using System.Reflection.Metadata;

internal class RestrictedSerializationBinder
{
    Dictionary<string, Type> AllowList { get; set; }

    RestrictedSerializationBinder(Type[] allowedTypes)
        => AllowList = allowedTypes.ToDictionary(type => type.FullName!);

    Type? GetType(ReadOnlySpan<char> untrustedInput)
    {
        if (!TypeName.TryParse(untrustedInput, out TypeName? parsed))
        {
            throw new InvalidOperationException($"Invalid type name: '{untrustedInput.ToString()}'");
        }

        if (AllowList.TryGetValue(parsed.FullName, out Type? type))
        {
            return type;
        }
        else if (parsed.IsSimple // It's not generic, pointer, reference, or an array.
            && parsed.AssemblyName is not null
            && parsed.AssemblyName.Name == "MyTrustedAssembly"
            )
        {
            return Type.GetType(parsed.AssemblyQualifiedName, throwOnError: true);
        }

        throw new InvalidOperationException($"Not allowed: '{untrustedInput.ToString()}'");
    }
}
```

## Regular Expressions

### `[GeneratedRegex]` on properties

The following partial method will be source generated with all the code
necessary to implement this Regex.

```csharp
[GeneratedRegex(@"\b\w{5}\b")]
static partial Regex FiveCharWord();
```

The following partial property is the property equivalent of the previous
example.

```csharp
[GeneratedRegex(@"\b\w{5}\b")]
static partial Regex FiveCharWordProperty { get; }
```

## Regex.EnumerateSplits

```csharp
foreach (string s in Regex.Split("Hello, world! How are you?", "[aeiou]"))
{
    Console.WriteLine($"Split: \"{s}\"");
}

ReadOnlySpan<char> input = "Hello, world! How are you?";
foreach (Range r in Regex.EnumerateSplits(input, "[aeiou]"))
{
    Console.WriteLine($"Split: \"{input[r]}\"");
}
```

## Serialization (System.Text.Json)

There are a few innovations here too, but I'll pass since you are using
`Newtonsoft`.

- Indentation options
- Default web options singleton
- JsonSchemaExporter
- Respect nullable annotations
- Require non-optional constructor parameters
- Order JsonObject properties
- Customize enum member names
- Stream multiple JSON documents

## Spans

### `File` helpers

The `File` class now has new helpers to easily and directly write
`ReadOnlySpan<char>`/`ReadOnlySpan<byte>` and
`ReadOnlyMemory<char>`/`ReadOnlyMemory<byte>` to files.

New `StartsWith<T>(ReadOnlySpan<T>, T)` and `EndsWith<T>(ReadOnlySpan<T>, T)`
extension methods have also been added for spans, making it easy to test whether
a `ReadOnlySpan<T>` starts or ends with a specific `T` value.

### params `ReadOnlySpan<T>` overloads

C# has always supported marking array parameters as params. This keyword enables
a simplified calling syntax. For example, the `String.Join(String, String[])`
method's second parameter is marked with params. You can call this overload
with an array or by passing the values individually:

```csharp
string result = string.Join(", ", new string[3] { "a", "b", "c" });
string result = string.Join(", ", "a", "b", "c");
```

For example, `String.Join` now includes the following overload, which implements
the new pattern: `String.Join(String, ReadOnlySpan<String>)`

### Enumerate over `ReadOnlySpan<char>.Split()` segments

```csharp
public static bool ListContainsItem(ReadOnlySpan<char> span, string item)
{
    foreach (Range segment in span.Split(','))
    {
        if (span[segment].SequenceEquals(item))
        {
            return true;
        }
    }

    return false;
}
```

## System.Guid

In `.NET 9`, you can create a `Guid` according to Version 7 via the new
`Guid.CreateVersion7()` and `Guid.CreateVersion7(DateTimeOffset)` methods.

## Threading

### Task.WhenEach

```csharp
using HttpClient http = new();

Task<string> dotnet = http.GetStringAsync("http://dot.net");
Task<string> bing = http.GetStringAsync("http://www.bing.com");
Task<string> ms = http.GetStringAsync("http://microsoft.com");

await foreach (Task<string> t in Task.WhenEach(bing, dotnet, ms))
{
    Console.WriteLine(t.Result);
}
```

## Interlocked.CompareExchange for more types

With `.NET 9`, the `Interlocked.CompareExchange` and `Interlocked.Exchange`
methods support atomic operations on a wider range of types. This makes it
possible to perform simultaneous operations on more data types.

In `.NET 9`, there are new overloads for atomically working with `byte`,
`sbyte`, `short`, and `ushort`.
