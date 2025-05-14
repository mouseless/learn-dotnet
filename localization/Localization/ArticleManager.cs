using Microsoft.Extensions.Localization;
using System.Reflection;

public class ArticleManager(IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create("Localization", Assembly.GetExecutingAssembly().GetName().Name!);

    public string GetArticleName(string author) =>
        _localizer["articleName", author];
}