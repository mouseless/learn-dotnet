# Nullable Usage

To enable nullable in projects

```xml
<Nullable>enable</Nullable>
<WarningsAsErrors>Nullable</WarningsAsErrors>
```

##  Type of a Nullable

Nullable Value Types ```int?``` creates Nullable<int>. When we want to get the
type of a nullable value type, _GetType()_ will return type of 
```Nullable<T>```. In order to get the type of the property, we should use 
```Nullable.GetUnderlyingType(T)``` static method to extract type info.

Nullable Reference Types are different than Value Types and they only 
instruct the compiler about members or parameters that they can be null. 
Therefore ```string?``` is not creating a ```Nullable<string>``` the way 
that ```int?``` creates a ```Nullable<int>```.

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
## Getting the value of a Nullable

### HasValue()

### GetValueOrDefault()

TBD

## Using Nullables in Code

### Don't use nullable for Dependency Injection

### Carefully use comparison operators with nullables

```csharp
public class Person
{
    ...
    public string? MiddleName {get;}

    public Person(string name, string? middleName = default)
    {
        ...
    }

    public string? InitialName()
    {
        return Name.Length > MiddleName?.Lenght ? Name : MiddleName;
    }
}

#main

var person = new Person("John");

Console.WriteLine(person.FullName()) // 

// Prints empty string due to operator behaviour with nullable
// comparison operators always return null if on side of the
// comparison is null.

```

### ! (null-forgiving) Operator

! operator should only be used if a member is known to be not null in the given
context.

```csharp

string? nullableString;

public void Print()
{
    if(nullableString is not null)
    {
        Console.WriteLine{$"The length of the nullable string is: {nullableString!}"}
    }
}

```

! operator can be used in unit tests to test behaviour against `null` parameters.

```csharp

public class Person
{
    ...

    public Person(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}

[TestFixure]
public class PersonTests
{
    [Test]
    public void Person_constructor_throws_exception_if_name_is_null()
    {
        Should.Throw<ArguementNullExceptions>(()=> new Person(null!));
    }
}

```


