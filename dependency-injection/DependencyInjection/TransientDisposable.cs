namespace DependencyInjection;

public class TransientDisposable : IDisposable
{
    readonly Func<Scoped> _getScoped;
    readonly ILogger<TransientDisposable> _logger;

    bool _disposed;

    public TransientDisposable(Func<Scoped> getScoped, ILogger<TransientDisposable> logger)
    {
        _getScoped = getScoped;
        _logger = logger;
    }

    internal async Task Process() => await Task.Delay(500);

    void IDisposable.Dispose()
    {
        if (_disposed) { return; }

        _logger.LogInformation($"disposing: {nameof(TransientDisposable)}");
        _getScoped().DoOtherStuff(nameof(TransientDisposable));
        _logger.LogInformation($"disposed: {nameof(TransientDisposable)}");

        _disposed = true;
    }
}
