# .NET 10 Research Notes

## Libraries

### Numeric ordering for string comparison

stringlerin sonuna göre sıralama yapıyor.

```csharp
StringComparer numericStringComparer = StringComparer.Create(CultureInfo.CurrentCulture, CompareOptions.NumericOrdering);

foreach (string os in new[] { "Windows 11", "Windows 10", "Windows 8" }.Order(numericStringComparer))
{
    Console.WriteLine(os);
}

// Output:
// Windows 8
// Windows 10
// Windows 11
```

This option isn't valid for the following index-based string operations:
IndexOf, LastIndexOf, StartsWith, EndsWith, IsPrefix, and IsSuffix.

## SDK

### SLNX file format

sln dosyaları slnx olacak

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

### Validation support in Minimal APIs

`AddValidation` ile response model'de eklenen attributes'lar ile validasyon yapılabiliyor

```csharp
app.MapPost("/products",
    ([EvenNumber(ErrorMessage = "Product ID must be even")] int productId, [Required] string name)
        => TypedResults.Ok(productId))
    .DisableValidation();
//******
public record Product(
    [Required] string Name,
    [Range(1, 1000)] int Quantity);
```

### `IProblemDetailsService`
bir örnek yap
Defines a type that provide functionality to create a ProblemDetails response.

Bu 7'den sonra da varmış. Kullanmamışız. Ben yinede ekliyim belki şimdi bazı şeyleri kolaylayabilir.

https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.iproblemdetailsservice?view=aspnetcore-10.0

### Validation APIs moved to `Microsoft.Extensions.Validation`

The validation APIs have moved to the `Microsoft.Extensions.Validation`
namespace and NuGet package.

### OpenAPI

#### Response description on ProducesResponseType for API controllers

The `ProducesAttribute`, `ProducesResponseTypeAttribute`, and
`ProducesDefaultResponseTypeAttribute` now accept an optional string parameter,
`Description`, that sets the description of the response:

```C#
[HttpGet(Name = "GetWeatherForecast")]
[ProducesResponseType<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK,
    Description = "The weather forecast for the next 5 days.")]
public IEnumerable<WeatherForecast> Get()
{
```

#### Support for `IOpenApiDocumentProvider` in the DI container

bu servis OpenAPI dokümanını kod içinden programatik olarak alabilmeyi sağlıyor

#### Use HTTP Method Object Instead of Enum

```csharp
// Before (1.6)
OpenApiOperation operation = new OpenApiOperation
{
    HttpMethod = OperationType.Get
};

// After (2.0)
OpenApiOperation operation = new OpenApiOperation
{
    HttpMethod = new HttpMethod("GET") // or HttpMethod.Get
};
```

[daha fazla 2.0 değişikliği için](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md)

### Authentication and authorization

#### Avoid cookie login redirects for known API endpoints

sanırım bu bizi ilgilendirmiyor biz login e redirect etmiyoruz zaten ama yapılan
şeyi yinede yazayım:
Artık bilinen API endpoint'lerine yapılan yetkisiz istekler login sayfasına
yönlendirme yerine doğrudan 401 ve 403 döndürüyor.

### Diğerleri

#### Support for the .localhost Top-Level Domain

```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://myapp.dev.localhost:7099;http://myapp.dev.localhost:5036"
    }
  }
}
```

> [!NOTE]
>
> After installing .NET 10 SDK Preview 7, trust the new developer certificate by
> running dotnet dev-certs https --trust at the command line to ensure your
> system is configured to trust the new certificate.

#### Detect if URL is local using `RedirectHttpResult.IsLocalUrl`

`RedirectHttpResult.IsLocalUrl` diye bir helper gelmiş. url veriyorsun oradaki
redirecturl locale e mi gidecek diye bakıyor. locale se true dönüyor.

## C# 14

### Extension members

yeni extension yazma yöntemleri geldi
bu syntax a geçilecek
```csharp
public static class Enumerable
{
    // Extension block
    extension<TSource>(IEnumerable<TSource> source) // extension members for IEnumerable<TSource>
    {
        // Extension property:
        public bool IsEmpty => !source.Any();

        // Extension method:
        public IEnumerable<TSource> Where(Func<TSource, bool> predicate) { ... }
    }

    // extension block, with a receiver type only
    extension<TSource>(IEnumerable<TSource>) // static extension members for IEnumerable<Source>
    {
        // static extension method:
        public static IEnumerable<TSource> Combine(IEnumerable<TSource> first, IEnumerable<TSource> second) { ... }

        // static extension property:
        public static IEnumerable<TSource> Identity => Enumerable.Empty<TSource>();

        // static user defined operator:
        public static IEnumerable<TSource> operator + (IEnumerable<TSource> left, IEnumerable<TSource> right) => left.Concat(right);
    }
}
```

### `field`

artık field tanımlamaya gerek yok

```csharp
public string Name
{
    get;
    set => field = value ?? throw new ArgumentNullException(nameof(value));
}
```

### Implicit Span Conversions

Span<T> ve ReadOnlySpan<T> için, ReadOnlySpan<T>, Span<T> ve T[] türlerine
implicit cast gelmiş

```csharp
int[] array = new[] { 1, 2, 3 };
ReadOnlySpan<int> ros = array;

void Process(ReadOnlySpan<int> data) { }
Process(array);
```

### `nameof` Unbound Generic Types

aşağıdaki işlemlere izin veriliyormuş

```csharp
nameof(List<>)
nameof(Dictionary<,>)
```

### Simple lambda parameters with modifiers

scoped, ref, in, out, or ref readonly gibi lambda expression parametrelerinde
type belirtme biraz daha hafiflemiş. bunlarda analyzerlar önerileri veriyor zaten

```csharp
TryParse<int> parse2 = (string text, out int result) => Int32.TryParse(text, out result);
```

### partial members

artık instance constructors ve events partial olabiliyormuş

### User-Defined Compound Assignment

sanırım eskiden +=, -=, *= gibi operatorleri kendin yazamıyordun. artık
yazılabiliyor gibi

tam olarak hangilerine izin var tam anlamadım https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-14.0/user-defined-compound-assignment

### Null-Conditional Assignment

atamalarda if ile not null kontrolüne gerek kalmamış

```csharp
customer?.Order = GetCurrentOrder();
```

ama +=, -= falan desteklemiyor