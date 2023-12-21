namespace DependencyInjection;

public class Transient(Singleton _singleton, Scoped _scoped, ILogger<Transient> _logger)
{
    readonly Guid _id = Guid.NewGuid();

    internal void DoStuff(string source)
    {
        _logger.LogInformation($"Transient[{_id}] is doing stuff from {source}");

        _singleton.DoOtherStuff("transient");
        _scoped.DoOtherStuff("transient");
    }
}
