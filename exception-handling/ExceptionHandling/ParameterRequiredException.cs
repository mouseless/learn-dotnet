namespace ExceptionHandling;

public class ParameterRequiredException : Exception
{
    public string Name { get; }

    public ParameterRequiredException(string name)
        : base($"{name} is required.")
    {
        Name = name;
    }
}