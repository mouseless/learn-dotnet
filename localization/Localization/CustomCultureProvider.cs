using Microsoft.AspNetCore.Localization;

public class CustomCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var language = httpContext.Request.Headers["My-Culture"].FirstOrDefault();
        if (string.IsNullOrEmpty(language))
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        return Task.FromResult<ProviderCultureResult?>(
            new ProviderCultureResult(language, language));
    }
}