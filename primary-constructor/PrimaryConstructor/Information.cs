namespace PrimaryConstructor;

public abstract class Information(string from)
{
    public string From { get; } = from;
    protected virtual void SendMessage(string @for, Notification notification) =>
        Console.WriteLine($"From {From} For {@for} => {notification.Title}: {notification.Content}");
}
