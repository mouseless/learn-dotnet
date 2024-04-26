namespace DependencyInjection;

public class Scoped(Singleton _singleton, Func<Transient> _newTransient, ILogger<Scoped> _logger)
{
    readonly Guid _id = Guid.NewGuid();

    public void DoStuff(string source)
    {
        _logger.LogInformation($"Scoped[{_id}] is doing stuff from ${source}");

        _singleton.DoOtherStuff("scoped");
        _newTransient().DoStuff("scoped");
    }

    public void DoOtherStuff(string source)
    {
        _logger.LogInformation($"Scoped[{_id}] is doing other stuff from {source}");
    }
}