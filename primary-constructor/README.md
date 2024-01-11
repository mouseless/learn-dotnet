# Primary Constructors

We use Primary Constructors to achieve a better representation of required
dependencies and initialization parameters and get rid of constructors with only
assignments and no logic to have a simpler code.

> ⚠️
>
> .NET 8 and C# 12 are required. If you are using Visual Studio and you get
> unsupported  warning, make sure you get update.

## Usage in Records

Since records use given parameter name as the name of its corresponding property
name, we use `PascalCase` in parameter names of record primary constructors.

```csharp
public record EmployeeRequest(string Name, string Surname);
```

## Usage in Classes

For classes there are two types of usages for a parameter of the primary
constructor, field and parameter.

### As Fields

If the parameter is directly used as a field, place an underscore (`_`) as a
prefix, just like a field name.

> 💡This is mostly the case for dependency injection.

```csharp
public class Salary(SalaryCalculator _calculator)
{
    public virtual decimal GetSalary(Employee employee) =>
        _calculator.Calculate(employee.Seniority);
}
```

### As Parameters

If a parameter is assigned to a property, use `camelCase`.

```csharp
public class Person(string name, string surname)
{
    public string Name { get; } = name;
    public string Surname { get; } = surname;
    public string FullName { get; } = $"{name} {surname}";
}
```

### Initializing Base Class

Derived classes must follow parameter names in base constructors.

```csharp
public class CustomException(string message)
    : Exception(message) { }

public class YearlySalary(ISalaryCalculator _calculator)
    : Salary(_calculator)
{
    public override decimal GetSalary(Employee employee) =>
        base.GetSalary(employee) * 12;
}
```

### Potential Conflicts

In practice, it should not be the case for a class to have parameters both as
fields and parameters. If you encounter such a case please consider refactoring
your code.

When both field and parameter usage is possible for a class, prefer the
parameter usage over field usage. Please see below demonstration;

```csharp
public class Employee(string name) // This is the preferred usage
    : IPerson
{
    string IPerson.Name { get; } = name;
}

public class Employee(string _name)
    : IPerson
{
    string IPerson.Name => _name;
}
```

## Constructor Overloads

If you need to add secondary constructors, you must use the `this` constructor
initializer as shown below.

```csharp
public class CustomException(string message, Exception innerException)
{
    public CustomException(string message)
       : this(message, default!) { }
}
```
