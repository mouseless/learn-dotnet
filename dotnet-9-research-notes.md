# .NET 9 Research Notes

## Topics to be learn

### OpenAPI

#### Built-in support for OpenAPI document generation

In .NET 9, ASP.NET Core provides built-in support for generating OpenAPI
documents representing controller-based or minimal APIs via the
`Microsoft.AspNetCore.OpenApi` package.

```csharp
var builder = WebApplication.CreateBuilder();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapGet("/hello/{name}", (string name) => $"Hello {name}"!);

app.Run();
```

OpenAPI documents can also be generated at build-time by adding the
`Microsoft.Extensions.ApiDescription.Server` package.

To modify the location of the emitted OpenAPI documents, set the target path in
the `OpenApiDocumentsDirectory` property in the app's project file:

```xml
<PropertyGroup>
  <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
</PropertyGroup>
```

### `ExceptionHandlerMiddleware` option to choose the status code based on the exception type

A new option when configuring the `ExceptionHandlerMiddleware` enables app
developers to choose what status code to return when an exception occurs during
request handling. The new option changes the status code being set in the
`ProblemDetails` response from the `ExceptionHandlerMiddleware`.

```csharp
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex is TimeoutException
        ? StatusCodes.Status503ServiceUnavailable
        : StatusCodes.Status500InternalServerError,
});
```

### `params` collections

The params modifier isn't limited to array types. You can now use params with
any recognized collection type, including `System.Span<T>`,
`System.ReadOnlySpan<T>`, and types that implement
`System.Collections.Generic.IEnumerable<T>` and have an Add method. In addition
to concrete types, the interfaces `System.Collections.Generic.IEnumerable<T>`,
`System.Collections.Generic.IReadOnlyCollection<T>`,
`System.Collections.Generic.IReadOnlyList<T>`,
`System.Collections.Generic.ICollection<T>`, and
`System.Collections.Generic.IList<T>` can also be used.

For example, method declarations can declare spans as params parameters:

```csharp
public void Concat<T>(params ReadOnlySpan<T> items)
{
    for (int i = 0; i < items.Length; i++)
    {
        Console.Write(items[i]);
        Console.Write(" ");
    }
    Console.WriteLine();
}
```

### `Base64Url`

Base64Url has been added because there are problems with special characters
(such as +, /) when Base64 is used for urls.

### Persisted assemblies

`.NET 9` introduces the `PersistedAssemblyBuilder` class, which supports
creating and saving dynamic assemblies. This brings the assembly-saving
capabilities from `.NET Framework` to `.NET`.

- **Assembly Saving**: Dynamically create types and methods, then save them as
  executable (.exe) or library (.dll) files.
- **PDB Support**: Add symbol information for debugging.
- **Advantage**: Simplifies migration from `.NET Framework` to `.NET 9` and
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

### Regular Expressions

#### `[GeneratedRegex]` on properties (methodlar property'e geçecek)

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

#### Regex.EnumerateSplits

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

## Topics to be checked for existence in the projects and updated if any

### Middleware supports Keyed DI

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddKeyedSingleton<MySingletonClass>("test");
builder.Services.AddKeyedScoped<MyScopedClass>("test2");

var app = builder.Build();
app.UseMiddleware<MyMiddleware>();
app.Run();

internal class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next,
        [FromKeyedServices("test")] MySingletonClass service)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context,
        [FromKeyedServices("test2")]
            MyScopedClass scopedService) => _next(context);
}
```

### Linq

In `Linq`, the new methods `CountBy` and `AggregateBy`, `Index<TSource>` added.

#### `CountBy`

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

#### `AggregateBy`

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

#### `Index<TSource>(IEnumerable<TSource>)`

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

### `BinaryFormatter` Removed

It is throwing exceptions, although there's still access.

### New `TimeSpan.From*` Overloads

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
