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
