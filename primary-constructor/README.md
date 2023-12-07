# Primary Constructor

We use the primary constructor to get the dependencies in the constructor of the
classes and to get rid of the assignment lines in the constructor that are not
logic and take up space.

> :warning:
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

```csharp
public class Person(IEntityContext<Person> _context)
{
    public virtual void Delete()
    {
        _context.Delete(this);
    }
}
```

```csharp
public record Stubber(Spec Spec);
```
