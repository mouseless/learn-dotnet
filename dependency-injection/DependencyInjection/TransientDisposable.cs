namespace DependencyInjection;

public class TransientDisposable : IDisposable
{
    readonly Func<ScopedWithFactory> _getScopedWithFactory;
    readonly ILogger<TransientDisposable> _logger;

    private bool _disposedValue = default!;

    public TransientDisposable(Func<ScopedWithFactory> getScopedWithFactory, ILogger<TransientDisposable> logger)
    {
        _getScopedWithFactory = getScopedWithFactory;
        _logger = logger;
    }

    internal async Task Process()
    {
        await Task.Delay(500);
    }

    protected virtual void Dispose(bool disposing)
    {
        _logger.LogInformation($"dispose called on: {nameof(TransientDisposable)}");

        if (!_disposedValue)
        {
            if (disposing)
            {
                try
                {
                    _logger.LogInformation($"disposing: {nameof(TransientDisposable)}");
                    _getScopedWithFactory().DoSomething();
                    _logger.LogInformation($"disposed: {nameof(TransientDisposable)}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"dispose failed: {ex.Message}");
                }
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class ScopedWithFactory
{
    internal void DoSomething() { }
}
