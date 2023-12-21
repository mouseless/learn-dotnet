using Microsoft.Extensions.Logging;

namespace CSharp;

public class LambdaParameters(ILogger<CollectionExpressions> _logger)
{
    public void OptionalParameters()
    {
        var action = (int value = 2) =>
            _logger.LogInformation($"{(value == 2 ? "Called with default value" : "Called with given value")}: {value}");

        action();
        action(5);
    }

    public void ParamsArrayParameters()
    {
        var action = (params int[] values) =>
            _logger.LogInformation($"Called with {values.Length} parameters");

        action(1, 2, 3, 4, 5);
    }

    public void NewAcceptedBehavior()
    {
        void Optional(int value = 2) { }
        void Params(params int[] values) { }

        var optional = Optional;
        var @params = Params;

        optional();
        @params();
    }
}
