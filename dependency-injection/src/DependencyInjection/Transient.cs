namespace DependencyInjection;

public class Transient
{
    readonly Singleton _singleton;
    readonly Scoped _scoped;
    readonly ILogger<Transient> _logger;
    readonly Guid _id = Guid.NewGuid();

    public Transient(Singleton singleton, Scoped scoped, ILogger<Transient> logger) =>
        (_singleton, _scoped, _logger) = (singleton, scoped, logger);

    internal void DoStuff(string source)
    {
        _logger.LogInformation($"Transient[{_id}] is doing stuff from {source}");

        _singleton.DoOtherStuff("transient");
        _scoped.DoOtherStuff("transient");
    }
}
