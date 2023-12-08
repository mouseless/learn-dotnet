# Primary Constructor

We use the primary constructor to get the dependencies in the constructor of the
classes and to get rid of the assignment lines in the constructor that are not
logic and take up space.

> :warning:
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

```csharp
public record MyRecord(object parameter);
```

```csharp
public class MyService(Dependency _dependency)
{
    public void DoStuff()
    {
        _dependency.DoStuff();
    }
}
```

> :bulb:
>
> If you need to give default values to the parameters, you can give them using
> `this` as shown below.
>
> Default
>
> ```csharp
> public class MyService(Dependency _dependency)
> {
>     protected MyService() : this(default!) { }
>
>     public void DoStuff()
>     {
>         _dependency.DoStuff();
>     }
> }
> ```
