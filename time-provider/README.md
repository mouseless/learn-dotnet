# Time Provider

We use the abstract `TimeProvider` class to get rid of the extra interfaces and
classes we previously wrote for `DateTime`, as it is more global and is attached
to the base class library to support a wide variety of .NET runtimes.

## Use in dependency injection

```csharp
var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(TimeProvider.System);
serviceCollection.AddSingleton<MyService>();

public class MyService(TimeProvider _timeProvider)
{
    public boolean IsMonday() =>
        _timeProvider.GetLocalNow().DayOfWeek == DayOfWeek.Monday;
}
```

## Use in tests

```csharp
var mock = new Mock<TimeProvider>();
var mockedTimeProvider = mock
    .Setup(x => x.GetLocalNow())
    .Returns(new DateTimeOffset(2025, 12, 31, 23, 59, 59, TimeSpan.Zero));;
var myService = new MyService(mockedTimeProvider);

var actual = myService.IsMonday();

actual.ShouldBeTrue();
```
