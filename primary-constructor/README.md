# Primary Constructors

We use Primary Constructors to achieve a better representation of required
dependencies and initialization parameters and get rid of constructors with only
assignments and no logic to have a simpler code.

> ⚠️
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

## Usage in Records

```csharp
public record EmployeeDTO(string Name, string Surname);
```

## Usage for Dependency Injection

```csharp
public class EmployeeService(IConfiguration _configuration)
{
    public void AddEmployee(EmployeeDTO employee)
    {
        _configuration.GetValue(...);
    }
}
```

## Initializing Property

```csharp
public class Person(string name, string surname)
{
    public string Name { get; } = name;
    public string Surname { get; } = surname;
    public string FullName { get; } = name + surname;
}
```

> ⚠️
>
> When you assign a parameter to initialize a property or a field you can not
> reference this parameter in methods or calculated properties.
>
> ```csharp
> // CS9124: Parameter 'xxx' is captured into the state of the enclosing type and its
> // value is also used to initialize a field, property, or event.
> public string FullName => name + surname;
> // or
> public string GetFullName() => name + surname;
> ```

## Initializing Base Class

```csharp
public class Employee(int branchId, string name, string surname)
    : Person(name, surname)
{
    public int BranchId { get; } = branchId;
}
```

> :warning:
>
> It is not possible to use a parameter both as a field within a class and as a
> parameter in the base class simultaneously.

## Secondary Constructors

If you need to add secondary constructors, you must use the `this` constructor
initializer as shown below.

```csharp
public class Assignment(Employee employee, DateTime dateTime)
{
    public Assignment(Employee employee, TimeProvider timeProvider)
       : this(employee, timeProvider.GetNow()) { }

    ...
}
```

## Naming Conventions

When using Primary Constructor, we use following naming conventions;

- If we use the parameter as a field in class, we use camel case with underscore
  prefix.
- If we assign the parameter to the property in the class and use that property,
  we use camel case without underscore prefix.
- If we do not use the parameter in the class but pass it to the base class, we
  make sure that the parameter has the same name as the parameter in the base
  class.
- In records, we use pascal case in parameter name.
