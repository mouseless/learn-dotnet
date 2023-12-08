using SystemTimeProvider = System.TimeProvider;

namespace TimeProvider.Service;

public class MyService(SystemTimeProvider _timeProvider)
{
    public void IsMonday() =>
        Console.WriteLine(_timeProvider.GetLocalNow().DayOfWeek == DayOfWeek.Monday);
}
