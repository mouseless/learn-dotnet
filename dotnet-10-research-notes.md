<!-- TODO - Bu Dosya epic in son işiyle birlikte sininlemelidir. Sadece research notelarını barınmak için vardır -->
# .NET 10 Research Notes

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

### Code coverage EnableDynamicNativeInstrumentation defaults to false

büyük ihtimalle bizi etkilemeyecek ama depend edilen kütüphanelerle ilgili problem olursa diye
not olarak ekliyorum.

coverage collect te EnableDynamicNativeInstrumentation false yapılmış. native araçlardan
coverage almada sorun çıkarıyor bu flag

### dotnet restore audits transitive packages

audit warningleri default olarak seviye atlatılmış. bizde warningler error olarak gösterildiğinden
restorelarda sorun yaratabilir.

```xml
<ItemGroup>
    <NuGetAuditSuppress Include="url" />
</ItemGroup>

yada

<TreatWarningsAsErrors>
  <WarningsNotAsErrors>NU1901;NU1902;NU1903;NU1904;$(WarningsNotAsErrors)</WarningsNotAsErrors>
</TreatWarningsAsErrors>
```

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

#### Exception diagnostics are suppressed when IExceptionHandler.TryHandleAsync returns true

`IExceptionHandler` ile exception'ları handle ettiğinde(true döndüğünde) artık o exception
için otomatik olarak log ve telemetry yazılmıyor

## C# 14

### `nameof` Unbound Generic Types

aşağıdaki işlemlere izin veriliyormuş

```csharp
nameof(List<>)
nameof(Dictionary<,>)
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
