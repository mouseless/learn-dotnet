using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

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
        options.RequestCultureProviders = [
            new CustomCultureProvider(),
            new AcceptLanguageHeaderRequestCultureProvider()
        ];
    });

builder.Services.AddSingleton<ArticleManager>();

var app = builder.Build();

app.UseRequestLocalization();

app.UseHttpsRedirection();

var articleManager = app.Services.GetService<ArticleManager>()!;

app.MapGet("/", () => "go to /article path by giving value to author in query");
app.MapGet("/article", ([FromQuery] string author) => articleManager.GetArticleName(author));

app.Run();