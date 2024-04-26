namespace DependencyInjection;

public class TransientDisposable(Func<Scoped> _getScoped, ILogger<TransientDisposable> _logger)
    : IDisposable
{
    bool _disposed;

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