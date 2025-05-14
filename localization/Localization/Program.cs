using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        options.DefaultRequestCulture = new(culture: "en-US", uiCulture: "en-US");
        options.SupportedCultures = [new("en-US"), new("tr-TR")];
        options.SupportedUICultures = [new("en-US"), new("tr-TR")];
        options.RequestCultureProviders =
        [
            new CustomCultureProvider(),
            new QueryStringRequestCultureProvider(),
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