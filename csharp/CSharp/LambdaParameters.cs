using Microsoft.Extensions.Logging;

namespace CSharp;

public class LambdaParameters(ILogger<LambdaParameters> _logger)
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

    private delegate bool TryParse<T>(string text, out T result);

    public void ParameterModifiersWithoutTypes()
    {
        // Lambdas support modifiers (ref/in/out/scoped/ref readonly) without explicit parameter types when the target delegate type is known
        TryParse<int> parse = (text, out result) => int.TryParse(text, out result);

        _logger.LogInformation($"(text, out result) => int.TryParse: ok={parse("42", out int value)}, result={value}");
    }
}