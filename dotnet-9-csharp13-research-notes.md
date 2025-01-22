# C# 13 Research Notes

## `params` collections

The params modifier isn't limited to array types. You can now use params with
any recognized collection type, including `System.Span<T>`,
`System.ReadOnlySpan<T>`, and types that implement
`System.Collections.Generic.IEnumerable<T>` and have an Add method. In addition
to concrete types, the interfaces `System.Collections.Generic.IEnumerable<T>`,
`System.Collections.Generic.IReadOnlyCollection<T>`,
`System.Collections.Generic.IReadOnlyList<T>`,
`System.Collections.Generic.ICollection<T>`, and
`System.Collections.Generic.IList<T>` can also be used.

For example, method declarations can declare spans as params parameters:

```csharp
public void Concat<T>(params ReadOnlySpan<T> items)
{
    for (int i = 0; i < items.Length; i++)
    {
        Console.Write(items[i]);
        Console.Write(" ");
    }
    Console.WriteLine();
}
```

## New lock object

The .NET 9 runtime includes a new type for thread synchronization, the
`System.Threading.Lock` type. This type provides better thread synchronization
through its API. The `Lock.EnterScope()` method enters an exclusive scope. The
`ref struct` returned from that supports the `Dispose()` pattern to exit the
exclusive scope.

## New escape sequence

You can use `\e` as a character literal escape sequence for the `ESCAPE`
character, Unicode `U+001B`. Previously, you used `\u001b` or `\x1b`. Using
`\x1b` wasn't recommended because if the next characters following `1b` were
valid hexadecimal digits, those characters became part of the escape sequence.

## Implicit index access

The implicit "from the end" index operator, ^, is now allowed in an object initializer expression. For example, you can now initialize an array in an object initializer as shown in the following code:

```csharp
public class TimerRemaining
{
    public int[] buffer { get; set; } = new int[10];
}

var countdown = new TimerRemaining()
{
    buffer =
    {
        [^1] = 0,
        [^2] = 1,
        [^3] = 2,
        [^4] = 3,
        [^5] = 4,
        [^6] = 5,
        [^7] = 6,
        [^8] = 7,
        [^9] = 8,
        [^10] = 9
    }
};
```

## `ref` and `unsafe` in iterators and `async` methods

In C# 13, `async` methods can declare `ref` local variables, or local variables
of a `ref struct` type. However, those variables can't be accessed across an
`await` boundary. Neither can they be accessed across a `yield return` boundary.

In the same fashion, C# 13 allows `unsafe` contexts in iterator methods.
However, all `yield return` and `yield break` statements must be in safe
contexts.

## `allows ref struct`

Generic type declarations can add an anti-constraint, `allows ref struct`. This
anti-constraint declares that the type argument supplied for that type parameter
can be a `ref struct` type. The compiler enforces ref safety rules on all
instances of that type parameter.

```csharp
public class C<T> where T : allows ref struct
{
    // Use T as a ref struct:
    public void M(scoped T p)
    {
        // The parameter p must follow ref safety rules
    }
}
```

## `ref struct` interfaces

You can declare that a `ref struct` type implements an interface. However, to
ensure ref safety rules, a `ref struct` type can't be converted to an interface
type.

## More partial members

You can declare partial properties and partial indexers in C# 13.

```csharp
public partial class C
{
    // Declaring declaration
    public partial string Name { get; set; }
}

public partial class C
{
    // implementation declaration:
    private string _name;
    public partial string Name
    {
        get => _name;
        set => _name = value;
    }
}
```

## Overload resolution priority

In C# 13, the compiler recognizes the `OverloadResolutionPriorityAttribute` to
prefer one overload over another. Library authors can use this attribute to
ensure that a new, better overload is preferred over an existing overload. For
example, you might add a new overload that's more performant. You don't want to
break existing code that uses your library, but you want users to update to the
new version when they recompile. You can use Overload resolution priority to
inform the compiler which overload should be preferred. Overloads with the
highest priority are preferred.

```csharp
using System.Runtime.CompilerServices;

public class Calculator
{
    // Varsayılan öncelik
    public int Add(int x, int y) => x + y;

    // Yeni, daha iyi overload
    [OverloadResolutionPriority(1)]
    public int Add(int x, int y, int z) => x + y + z;
}

//or
using System.Runtime.CompilerServices;

var d = new Derived();
d.M([1, 2, 3]); // Prints "Derived", because members from Base are not considered due to finding an applicable member in Derived

class Base
{
    [OverloadResolutionPriority(1)]
    public void M(ReadOnlySpan<int> s) => Console.WriteLine("Base");
}

class Derived : Base
{
    public void M(int[] a) => Console.WriteLine("Derived");
}
```
