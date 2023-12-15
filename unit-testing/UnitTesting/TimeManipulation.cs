using Microsoft.Extensions.Time.Testing;

namespace UnitTesting;

public class TimeManipulation : Spec
{
    class Information(TimeProvider _timeProvider)
    {
        public bool SendMails() =>
            _timeProvider.GetLocalNow().Day == 8;
    }

    [Test]
    public void Mails_are_sent_on_the_8th()
    {
        var fakeTime = new FakeTimeProvider();
        fakeTime.SetUtcNow(new DateTimeOffset(2025, 12, 8, 23, 59, 59, TimeSpan.Zero));
        var days = new Information(fakeTime);

        days.SendMails().ShouldBeTrue();
    }
}
