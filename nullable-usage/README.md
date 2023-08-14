# Nullable Usage

To enable nullable in projects

```xml
<Nullable>enable</Nullable>
<WarningsAsErrors>Nullable</WarningsAsErrors>
```

Nullable Reference Types are only a compiler instruction intended to prevent warnings about objects potentially being null in a #nullable conext.

string? is not creating a Nullable<string> the way that int? creates a Nullable<int>. 

## Geting Type of a nullable

N/A

## Nullable Value

### HasValue()

### GetValueOrDefault()

TBD

## ! (null-forgiving) Operator

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

! operator can be used in unit tests to test behaviour against `null` parameters

```csharp

public class Person
{
    public string Name { get; }

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


