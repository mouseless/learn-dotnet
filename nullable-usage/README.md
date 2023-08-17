# Nullable Usage

Nullable types gives the ability to the compiler to check for possible 
null return values or not assigned members to detect possibility of any
_NullPointerException_ which may ocuur at runtime.

Also with the usage of nullable syntax, it is easier to visually see
the intention of the code about the certaion issue ,

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
public class PersonService
{
    readonly IAuthenticationContext _authenticationContext;
    readonly IQueryContext<Person>? _queryContext; // Invalid use of nullable
    ...

    public PersonService(IAuthenticationContext authenticationContext, IQueryContext<Person>? queryContext)
    {
        _authenticationContext = authenticationContext;
        _queryContext = queryContext;
    }
}
```

> :information_source:
>
> The DI container will resolve every dependency before initalizing the object
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
public class Person
{
    readonly IEntityContext<Person> _context = default!;

    protected Person() { }
    public Person(IEntityContext<Person> context)
    {
        _context = context;
    }

    public string Name { get; }
    public string? MiddleName {get; }
    public int? Age {get; }

    public virtual Person With(string name, string? middleName, int? age)
    {
        ...
    }
}
```

> :information_source:
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
    public string Name { get; }
    public string? MiddleName {get;}
    public int? Age {get;}

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
public class PersonService
{
    public void AddPerson(string? name)
    {
        if(name is null) throw new ArgumentNullException();

        _newPerson().With(name);
    }
}
```

#### Assign a default value if parameter is nullable
 
```csharp
public class PersonService
{
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
    public string Name { get; set; } = default!;
    public string? MiddleName { get; set; }
    ...

    public virtual Person With(string name, string? middleName)
    {
        Name = name;
        MiddleName = middleName;
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
public class Person
{
    ...

    public void ChangeMiddleName(string middleName)
    {
        ...
    }
}

public class PersonService
{
    public void UpdatePerson(
        Person person,
        string? middleName = default
    )
    {
        if(middleName is null) throw new ArgumentNullException();

        person.ChangeMiddleName(middleName)
    }
}
```

#### Assign default value

```csharp
public class Person
{
    ...

    public void ChangeMiddleName(string middleName)
    {
        ...
    }
}

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
    public string Name { get; }
    public string? MiddleName {get;}
    public int? Age {get;}
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
    ...
    public string? MiddleName {get;}

    public Person(string name, string? middleName = default)
    {
        ...
    }

    public string? InitialName =>
        return Name.Length > MiddleName?.Length ? Name : MiddleName;
}

#main

var person = new Person("John");

Console.WriteLine(person.FullName()) // 

// Prints empty string because MiddleName is null and
// comparison result is false

```