using Microsoft.Extensions.Logging;

namespace CSharp;

public class Params(ILogger<Params> _logger)
{
    public void Use()
    {
        ArrayParam("arg1", "arg2", "arg3");

        EnumerableParam("arg1", "arg2", "arg3");
    }

    void ArrayParam(params string[] args)
    {
        _logger.LogInformation($"This method is called with {string.Join(',', args)} which is a string[]");
    }

    void EnumerableParam(params IEnumerable<string> args)
    {
        _logger.LogInformation($"This method is called with {string.Join(',', args)} which is an IEnumerable<string>");
    }
}