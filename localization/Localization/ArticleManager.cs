using Microsoft.Extensions.Localization;

public class ArticleManager(IStringLocalizer<ArticleManager> _localizer)
{
    public string GetArticleName() => _localizer["articleName"];
}
