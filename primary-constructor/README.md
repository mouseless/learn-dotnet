# Primary Constructors

We use Primary Constructors to achieve a better representation of required
dependencies and initialization parameters and get rid of constructors with only
assignments and no logic to have a simpler code.

> :warning:
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

## Usage in Records

```csharp
public record MyRecord(object Parameter);
```

## Usage for Dependency Injection

When using the primary constructor for dependency injection.

```csharp
public class MyService(Dependency _dependency)
    : ServiceBase(_dependency)
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
> public class MyService(Dependency _dependency)
>    : ServiceBase(_dependency)
> {
>     protected MyService() : this(default!) { }
>
>     public void DoStuff()
>     {
>         _dependency.DoStuff();
>     }
> }
> ```

## Initializing Base Class

When using the primary constructor with base class.

```csharp
public class MyException(string message)
    : Exception(message) { }
```

## Naming Conventions

When using Primary Constructor, we use following naming conventions;

- If we use the parameter as a field in class, we use camel case with underscore
  prefix.
- If we equate the parameter to the property in the class and use that property,
  we use camel case without underscore prefix.
- If we do not use the parameter in the class but pass it to the base class, we
  make sure that the parameter has the same name as the parameter in the base
  class.
- In records, we use initial case in parameter name.
