# .NET 10 Research Notes

## Runtime

[Runtime'daki değişiklikler](https://learn.microsoft.com/tr-tr/dotnet/core/whats-new/dotnet-10/runtime)
doğrudan uygulanıyor.

## Libraries

### Numeric ordering for string comparison

stringlerin sonuna göre sıralama yapıyor.

```csharp
foreach (string os in new[] { "Windows 11", "Windows 10", "Windows 8" }.Order(numericStringComparer))
{
    Console.WriteLine(os);
}

// Output:
// Windows 8
// Windows 10
// Windows 11
```

### Serialization

Bir kaç işimize yarayabilecek serileştirme güncellemesi var ama biz Newtonsoft
kullanıyoruz.

https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/libraries#serialization

## SDK

### Support for Microsoft Testing Platform in dotnet test

dotnet test natively supports Microsoft.Testing.Platform. To enable this
feature, add the following configuration to your global.json file:

```csharp
{
    "test": {
        "runner": "Microsoft.Testing.Platform"
    }
}
```

[MTP](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro)

## ASP.NET

### Treating empty string in form post as null for nullable value types

When using the [FromForm] attribute
```csharp
app.MapPost("/todo", ([FromForm] Todo todo) => TypedResults.Ok(todo));

...

public class Todo
{
  public DateOnly? DueDate { get; set; } // Empty strings map to `null`
}
```