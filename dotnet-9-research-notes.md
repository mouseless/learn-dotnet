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

TBD...
