using Microsoft.Extensions.Logging;

namespace CSharp;

public class PropertiesAndFields(ILogger<PropertiesAndFields> _logger)
{
    string _field = "field";

    public string WithOutFieldKeyword { get => _field; set => _field = value; }
    public string WithFieldKeyword { get; set => field = value.Trim(); } = "WithField";

    public void FieldKeyword()
    {
        _logger.LogInformation($"_field => '{_field}'");
        _logger.LogInformation($"WithOutFieldKeyword => '{WithOutFieldKeyword}'");
        _logger.LogInformation($"WithFieldKeyword => '{WithFieldKeyword}'");
    }
}