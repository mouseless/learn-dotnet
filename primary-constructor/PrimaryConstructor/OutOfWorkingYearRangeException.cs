public class OutOfWorkingYearRangeException(string message, Exception? innerException)
    : Exception(message, innerException)
{
    public OutOfWorkingYearRangeException(string message)
        : this(message, default) { }
}