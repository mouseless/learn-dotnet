namespace DependencyInjection;

public class Scoped
{
    readonly Singleton _singleton;
    readonly Func<Transient> _newTransient;
    readonly ILogger<Scoped> _logger;

    public Scoped(Singleton singleton, Func<Transient> newTransient, ILogger<Scoped> logger) =>
        (_singleton, _newTransient, _logger) = (singleton, newTransient, logger);

    public void DoStuff(string source)
    {
        _logger.LogInformation($"Scoped is doing stuff from ${source}");

        _singleton.DoOtherStuff("scoped");
        _newTransient().DoStuff("scoped");
    }

    public void DoOtherStuff(string source)
    {
        _logger.LogInformation($"Scoped is doing other stuff from ${source}");
    }
}
