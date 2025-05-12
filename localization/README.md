# Localization

We use Localization to return the equivalents of string values, for example
exception messages, in the region and language of the user's choice.

## Setup

We register the localization with `.AddLocalization` and use it with
`.UseRequestLocalization`.

## Configure

You can configure culture settings using `RequestLocalizationOptions`:

```csharp
builder.Services.Configure<RequestLocalizationOptions>(...)
```

### RequestLocalizationOptions

This options class allows you to change the following configurations:

**options.DefaultRequestCulture**: This setting determines which culture the
application should use when no culture info is found in the request (e.g., no
`Accept-Language` header, no culture query string, no culture cookie).

```csharp
options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
```

**options.SupportedCultures**: This list specifies the cultures that the
application supports for date/number formatting and similar operations.

```csharp
var supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-US"),
    new CultureInfo("tr-TR")
};

options.SupportedCultures = supportedCultures;
```

This setting ensures that the app only works with the specified cultures. If a
culture like `en-US` is requested but not supported, the `DefaultRequestCulture`
will be used as a fallback.

**options.SupportedUICultures**: This list defines which cultures the
application supports for UI string translations. It is usually the same as
`SupportedCultures`, but it can differ. For instance, some cultures may support
only text translations but not date/number formatting.

```csharp
options.SupportedUICultures = supportedCultures;
```

**options.RequestCultureProviders**: These are mechanisms that determine which
culture to use for incoming requests.

```csharp
options.RequestCultureProviders = [
    new MyCustomCultureProvider(),
    new CookieRequestCultureProvider(),
    new AcceptLanguageHeaderRequestCultureProvider()
];
```

You can examine them in more detail under [Culture Providers](#culture-providers).

## Usage

### IStringLocalizer

To use localized strings within classes, use `IStringLocalizer<T>`. This will
look for `.resx` files that match the type `T`:

```csharp
public class ArticleManager(IStringLocalizer<ArticleManager> _localizer)
{
    public string GetArticleName() => _localizer["articleName"];
}
```

### IStringLocalizerFactory

If you want to read `.resx` files from a different folder, use
`IStringLocalizerFactory`:

```csharp
public class ArticleManager(IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(
        "ArticleManager",
        Assembly.GetExecutingAssembly().GetName().Name
    );
}
```

## Culture Providers

Culture providers are mechanisms that determine which culture will be used for
each request. `ASP.NET Core` uses the following three providers by default in
order:

1. **QueryStringRequestCultureProvider** → Culture is determined via query
   strings like `?culture=tr-TR`
1. **CookieRequestCultureProvider** → Remembers the user's culture using browser
   cookies
1. **AcceptLanguageHeaderRequestCultureProvider** → Looks at the
   `Accept-Language` header in `HTTP` requests

You can change this order or add a custom provider:

```csharp
options.RequestCultureProviders = [
    new CookieRequestCultureProvider(),
    new QueryStringRequestCultureProvider(),
    new AcceptLanguageHeaderRequestCultureProvider()
];
```

To write a custom provider, extend the `RequestCultureProvider` class:

```csharp
public class MyCustomCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var language = httpContext.Request.Headers["X-Language"].FirstOrDefault();
        if (!string.IsNullOrEmpty(language))
        {
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(language, language));
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}
```

Then include it in the list:

```csharp
options.RequestCultureProviders = [
    new MyCustomCultureProvider(),
    new CookieRequestCultureProvider(),
    new AcceptLanguageHeaderRequestCultureProvider()
];
```

## Resource Files

Text translations are provided via `.resx` files. The file naming convention is:

```txt
<FullTypeName>.resx         → Default language
<FullTypeName>.<Culture>.resx → Specific cultures
```

> [!NOTE]
>
> Instead of `.resx`, a database can also be used as a source.

### Parameterized Usage

You can use placeholders like `{0}`, `{1}` in `.resx` files:

```xml
<data name="platformName" xml:space="preserve">
  <value>Streaming Platform {0:P}</value>
</data>
```
