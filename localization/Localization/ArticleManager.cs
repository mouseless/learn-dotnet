using System.Reflection;
using Microsoft.Extensions.Localization;

public class ArticleManager(IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create("Terms", Assembly.GetExecutingAssembly().GetName().Name!);

    public string GetArticleName() => _localizer["articleName"];
}