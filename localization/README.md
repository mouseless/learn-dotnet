# Localization

nedir bu ....

## kurulum

Services.AddLocalization(); ile ekleniyor

daha sonra tabi

app.UseRequestLocalization();

çalırarak middleware'i kullanmamız gerekiyor.

## configure

builder.Services.Configure<RequestLocalizationOptions> ile configure edebiliyoruz.

verilen option ile default olarak geçerli olacak culture u belirleyebiliriz
options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

hangi bölgelerde hangi kultureleri kullanacağını options.SupportedCultures ve options.SupportedUICultures = supportedCultures; ile ayarlıyoruz

options.SupportedCultures = supportedCultures;
options.SupportedUICultures = supportedCultures;

aşağıda tam bir örnek görülmekte
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("tr-TR")
        };

        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

## kullanımlar

classlarda servisi almak için IStringLocalizer implament ediyoruz.
burada önemli olan IStringLocalizer e type verdiğimizde eğer configurasyon yapmassak type ile aynı isimde dosya arayacaktır.

public class ArticleManager(IStringLocalizer<ArticleManager> _localizer)
{
    public string GetArticleName() => _localizer["articleName"]; // loking for ArticleManager.*.resx
}

farklı yapılandırmalar için aşağıdaki yapılandırma gereklidir

public class ArticleManager(IStringLocalizerFactory factory _factory)
{
    private readonly IStringLocalizer _localizer = factory.Create("ArticleManager", Assembly.GetExecutingAssembly().GetName().Name);

    public string GetArticleName() => _localizer["articleName"]; // loking for ArticleManager.*.resx
}

isteklerin header'dan Accept-Language bölümünü değiştirerek farklı bölgeler için test edilebilir.

string localizer de parametrede verilebiliyor. örneğin

public string GetArticleName(string id) => _localizer["articleName", id];

bu parametrelerin karşılık bulması için resx teki dosyanında ona göre düzenlenmesi gerekir. #şurayabakın

## Culture Providers

bunlar atılan istekte hangi culture kullanacağını belirletilen özelliklerdir.

QueryStringRequestCultureProvider => ?culture=tr-TR gibi bir sorgu parametresinden kültür belirler
CookieRequestCultureProvider => Kullanıcının tarayıcısına ayarlanan kültürü saklar
AcceptLanguageHeaderRequestCultureProvider => Accept-Language başlığından kültür çeker

bu 3 provider sırası ile default olarak eklidir ve istekte sırayla bakılır ve ilk hangisi ile eşleşiyorsa onu kullanır.

custom provider ile kendi belirleyicini yaratabilirsin. bunun için RequestCultureProvider kullanmalısın

public class MyCustomCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var language = httpContext.Request.Headers["X-Language"].FirstOrDefault();
        if (!string.IsNullOrEmpty(language))
        {
            return Task.FromResult<ProviderCultureResult?>(
                new ProviderCultureResult(language, language));
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}

options.RequestCultureProviders = new List<IRequestCultureProvider>
{
    new MyCustomCultureProvider(),
    new CookieRequestCultureProvider(),
    new AcceptLanguageHeaderRequestCultureProvider()
};

## resousece lar

.resx formatı ile verilebilir,

<FullTypeName><.Locale>.resx

### şurası

resx dosyalarında parametre kullanmak için aşağıdaki örnekte olduğu gibi '{}' içinde parametre istenmelidir.

```xml
<data name="platformName" xml:space="preserve">
  <value>Yayın Aracı {0:P}</value>
</data>
```

uyarı! bu resx dosyaları embedded resource olmalı.

isteğe bağlı db de kullanılabiliyormuş...
