using Microsoft.Extensions.Logging;

namespace CSharp;

public class Assignments(ILogger<Assignments> _logger)
{
    public Data? NullableData { get; set; } = default;

    public void NullConditional()
    {
        NullableData?.Value = "test";

        _logger.LogInformation($"Before data is created, NullableData: {NullableData}");

        NullableData = new();
        NullableData?.Value = "test";

        _logger.LogInformation($"After data is created, NullableData: {NullableData} - Value: {NullableData?.Value}");
    }

    public class Data
    {
        public string? Value { get; set; }
    }
}