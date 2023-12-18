using Microsoft.Extensions.Logging;

namespace CSharp;

public class DefaultLambdaParameters(ILogger<CollectionExpressions> _logger)
{
    public void DefaultParameters()
    {
        var incrementValue = (int value = 2) =>
        {
            _logger.LogInformation($"{(value == 2 ? "Called with default value" : "Called with given value")}: {value}");

            return value + 1;
        };

        incrementValue();
        incrementValue(5);

        var action = incrementValue;
        action();
    }

    public void ParamsArrayParameters()
    {
        var sumValues = (params int[] values) =>
        {
            _logger.LogInformation($"Called with {values.Length} parameters");

            return values.Sum();
        };

        sumValues(1, 2, 3, 4, 5);

        int[] intArray = [1, 2, 3, 4, 5];
        List<int> intList = [1, 2, 3, 4, 5];

        sumValues([.. intArray, .. intList, 1]);
    }
}
