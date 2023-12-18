namespace DependencyInjection;

public class Singleton
{
    readonly Func<Scoped> _getScoped;
    readonly Func<Transient> _newTransient;
    readonly Func<TransientDisposable> _newTransientdisposable;
    readonly ILogger<Singleton> _logger;
    readonly Guid _id = Guid.NewGuid();

    public Singleton(Func<Scoped> getScoped, Func<Transient> newTransient, ILogger<Singleton> logger, Func<TransientDisposable> newTransientdisposable)
    {
        (_getScoped, _newTransient, _logger) = (getScoped, newTransient, logger);
        _newTransientdisposable = newTransientdisposable;
    }

    public void DoStuff(string source)
    {
        _logger.LogInformation($"Singleton[{_id}] is doing stuff from ${source}");

        _getScoped().DoStuff("singleton");
        _newTransient().DoStuff("singleton");
    }

    public void DoOtherStuff(string source)
    {
        _logger.LogInformation($"Singleton[{_id}] is doing other stuff from {source}");
    }

    public async Task TestTransientDisposable()
    {
        var transientDisposable = _newTransientdisposable();

        await transientDisposable.Process();
    }

    public async Task TestTransientDisposableWithUsing()
    {
        using var transientDisposable = _newTransientdisposable();

        await transientDisposable.Process();
    }
}
