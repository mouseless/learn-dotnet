namespace ExceptionHandling;

public class ParameterRequiredException : Exception
{
    public ParameterRequiredException(string? nameOfParameter)
        : base($"{nameOfParameter} is required.") { }
}