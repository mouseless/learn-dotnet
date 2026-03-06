using Microsoft.Extensions.Logging;

namespace CSharp;

public class PropertiesAndFields(ILogger<PropertiesAndFields> _logger)
{
    string _filed = "field";

    public string WithOutFieldKeyword { get => _filed; set => _filed = value; }
    public string WithFieldKeyword { get; set => field = value.Trim(); } = "WithField";

    public void FieldKeyword()
    {
        _logger.LogInformation($"_field => '{_filed}'");
        _logger.LogInformation($"WithOutFieldKeyword => '{WithOutFieldKeyword}'");
        _logger.LogInformation($"WithFieldKeyword => '{WithFieldKeyword}'");
    }
}
