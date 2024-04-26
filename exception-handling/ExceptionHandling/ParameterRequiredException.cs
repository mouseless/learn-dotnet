namespace ExceptionHandling;

public class ParameterRequiredException(string name)
    : Exception($"{name} is required.")
{
    public string Name { get; } = name;
}