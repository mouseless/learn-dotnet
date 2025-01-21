# .NET 9

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

TBD...
