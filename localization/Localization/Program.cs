using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddSingleton<PlatformManager>();

var app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var articleManager = app.Services.GetService<ArticleManager>()!;
var platformManager = app.Services.GetService<PlatformManager>()!;

app.MapGet("/", () => "Use '/article' or '/platform' for test");
app.MapGet("/article", () => articleManager.GetArticleName());
app.MapGet("/platform", ([FromQuery] string name) => platformManager.GetPlatform(name));

app.Run();