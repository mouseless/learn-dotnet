using Microsoft.Extensions.Localization;

public class PlatformManager(IStringLocalizer<Platform> _localizer)
{
    public string GetPlatform(string name) => _localizer["platformName", name];
}

public class Platform;