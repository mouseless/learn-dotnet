namespace DependencyInjection;

public class Singleton(Func<Scoped> _getScoped, Func<Transient> _newTransient, ILogger<Singleton> _logger, TimeProvider _timeProvider)
{
    readonly Guid _id = Guid.NewGuid();

    public void DoStuff(string source)
    {
        _logger.LogInformation($"{_timeProvider.GetUtcNow()}: Singleton[{_id}] is doing stuff from ${source}");

        _getScoped().DoStuff("singleton");
        _newTransient().DoStuff("singleton");
    }

    public void DoOtherStuff(string source)
    {
        _logger.LogInformation($"{_timeProvider.GetUtcNow()}: Singleton[{_id}] is doing other stuff from {source}");
    }
}
