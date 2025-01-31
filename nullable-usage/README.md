# Nullable Usage

Nullable types gives the ability to the compiler to check for possible
null return values or not assigned members to detect possibility of any
_NullPointerException_ which may occur at runtime.

Also with the usage of nullable syntax, it is easier to visually see
the intention of the code about the certain issue,

To enable nullable in projects make sure you add the following configuration
to your _.csproj_ file.

```xml
<Nullable>enable</Nullable>
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
```

## Using Nullables in Mouseless

We follow the guidelines below when writing code to properly use nullable syntax.

### Don't use nullable for Dependency Injection

```csharp
public class PersonService(IQueryContext<Person>? queryContext)
{
    ...
}
```

> [!NOTE]
>
> The DI container will resolve every dependency before initializing the object
> and an exception will be thrown for a if a component is not registered.
> Therefore, there is no possibility of dependency to be null.

### Avoid using ! (null-forgiving) operator

The usage of ! operator negates the null check that the compiler performs, so
the whole intention of enabling nullable check will be compromised and it
will be very hard to spot possible bugs which may occur at runtime.

! operator should only be used to dismiss the compiler errors when we set
default values for non nullable members which are;

- Assigned with dependency injection
- Assigned in the builder method

```csharp
public class Person(IEntityContext<Person> _context)
{
    protected Person() : this(default!) { }
    ...
}
```

> [!NOTE]
>
> Because of NHibernate, entities need a _protected_ parameterless constructor
> and compiler will highlight an error stating that the value of __context_ is
> not assigned when leaving a constructor. Therefore we need to assign a
> default value to __context_ to dismiss the compiler error.

### Always manage nullable parameters

When using nullable parameters, the main challenge is to manage what happens
within the scope of the method if a parameter has not been assigned with any
value.

We should always make sure the functionality is not broken and cause a wrong
intention wether a parameter is necessary or not.

#### Use not nullable when parameter is required

```csharp
public class Person
{
    public string Name { get; } = default!;

    ...

    // name is made not nullable to give responsibility to caller
    public virtual Person With(string name)
    {
        Name = name;
    }
}
```

### Handling null to not null situation

#### Check for value and throw a relevant exception

```csharp
public record AddPerson(string? Name);

public class PersonService : IPersonService
{
    ...

    public void AddPerson(string name) => _newPerson().With(name);

    void IPersonService.AddPerson(AddPerson data)
    {
        if(data.Name is null) throw new ArgumentNullException();

        AddPerson(data.Name);
    }
}
```

#### Assign a default value if parameter is nullable

```csharp
public class PersonService
{
    ...

    public void AddPerson(string? name)
    {
        name ??= "John Doe";

        _newPerson().With(name);
    }
}
```

#### Optional parameter

```csharp
public class PersonService
{
    ...

    public void AddPerson(
        string? name = default
    )
    {
        name ??= "John Doe";

        _newPerson().With(name);
    }
}
```

### Passing nullable value a method

#### Don't do anyting if parameter is already nullable

```csharp
public class Person
{
    ...

    public virtual Person With(string name, string? middleName)
    {
        ...
    }
}

public class PersonService
{
    public void AddPerson(
        string? name = default,
        string? middleName = default
    )
    {
        name ??= "John Doe";

        _newPerson().With(name, middleName);
    }
}
```

#### Throw relevant exception if parameter is required

```csharp
public class PersonService
{
    public void UpdatePerson(string? middleName = default)
    {
        if(middleName is null) throw new ArgumentNullException();
    }
}
```

#### Assign default value

```csharp
public class PersonService
{
    public void UpdateEntity(
        Person person,
        string? middleName = default
    )
    {
        middleName ??= "Mike";

        person.ChangeMiddleName(middleName)
    }
}
```

### Using ?. (null-check) operator

#### Dont use ?. operator for if affects request-response

```csharp
public class PersonService{

    ...

    public void DeletePerson(int id)
    {
        _queryContext.SingleById(id)?.Delete();
    }

    // Even the relevant entity may not exits, the result
    // code will be 200 and it will cause a miss direction
    // Throw an exception instead
}
```

#### Use ?. operator for intended nullables

```csharp
public class Person{
    ...
    Child? Child { get; set; }

    ...

    public void Delete()
    {
        Child?.Delete();
        ...
    }
}
```

### Type of a nullable

Nullable Reference Types are different than Value Types and they only
instruct the compiler about members or parameters that they can be null.
Therefore
 - ```string?``` is not a ```Nullable<string>```
 - ```int?``` is a ```Nullable<int>```

and `Nullable.GetUnderlyingType(T)` for reference types are null.

```csharp
public class Person
{
    public string Name { get; } = default!;
    public string? MiddleName { get; }
    public int? Age { get; }
}

...

Type middleNamePropertyType = typeof(Person).GetProperty("MiddleName").PropertyType;

Console.WriteLine(middleNamePropertyType); //System.String
Console.WriteLine(Nullable.GetUnderlyingType(middleNamePropertyType)); //

Type agePropertyType = typeof(Person).GetProperty("Age").PropertyType;

Console.WriteLine(agePropertyType); // System.Nullable`1[System.Int32]
Console.WriteLine(Nullable.GetUnderlyingType(agePropertyType)); // System.Int32

```

### Comparison operators with nullables

When used for comparing nullables, the operator returns false if one side
of the comparison is null.

See official [Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types#lifted-operators)
documentation for further details.

```csharp
public class Person
{
    public string? InitialName =>
        Name.Length > MiddleName?.Length ? Name : MiddleName;
}

#main

var person = new Person(name: "John");

Console.WriteLine(person.FullName()) //

// Prints empty string because MiddleName is null and
// comparison result is false

```
