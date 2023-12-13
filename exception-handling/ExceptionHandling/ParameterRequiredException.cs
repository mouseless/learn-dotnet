namespace ExceptionHandling;

public class ParameterRequiredException : Exception
{
    public string Name { get; set; } = "Parameter Required";

    public ParameterRequiredException(string name)
        : base($"{name} is required.") { }
}