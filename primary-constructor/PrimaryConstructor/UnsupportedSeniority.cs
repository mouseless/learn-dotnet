public class UnsupportedSeniority(string message, Exception innerException)
    : Exception(message, innerException)
{
    public UnsupportedSeniority(string message)
        : this(message, default!) { }
}