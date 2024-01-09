# Primary Constructor

We use primary constructor in records, dependency injection and exceptions. This
is to get the dependencies and parameter in the constructor of the classes and
to get rid of the assignment lines in the constructor that are not logic and
take up space.

> :warning:
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

## Usage in Records

```csharp
public record MyRecord(object Parameter);
```

## Usage for Dependency Injection

When using the primary constructor for dependency injection, we put an
underscore at the beginning of the parameter names.

```csharp
public class MyService(Dependency _dependency, OtherDependency otherDependency)
    : ServiceBase(_dependency, otherDependency)
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
> ```csharp
> public class MyService(Dependency _dependency, OtherDependency otherDependency)
>    : ServiceBase(_dependency, otherDependency)
> {
>     protected MyService() : this(default!) { }
>
>     public void DoStuff()
>     {
>         _dependency.DoStuff();
>     }
> }
> ```

## Usage for Exceptions

When using the primary constructor in exceptions, we initialize the parameter
names with a lowercase letter

```csharp
public class MyException(string message)
    : Exception(message) { }
```

## Naming Rules

When using Primary Constructor, we pay attention to some naming conventions.
These are

- If we use the parameter as a field in class, we name it with the prefix `_`.
- If we equate the parameter to the property in the class and use that property,
  we do not use the underscore prefix.
- If we do not use the parameter in the class but pass it to the base class, we
  make sure that the parameter is the same as the parameter name in the base
  class.
