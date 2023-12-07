using Microsoft.Extensions.Logging;

namespace PrimaryConstructor;

public class LogSystem(ILogger<LogSystem> _logger)
{
    public void TestLog(string message)
    {
        _logger.LogInformation(message);
    }
}
