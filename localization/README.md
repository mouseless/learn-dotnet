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

## resousece lar

.resx formatı ile verilebilir, isteğe bağlı db de kullanılabiliyormuş...