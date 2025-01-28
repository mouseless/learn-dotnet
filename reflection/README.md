# Reflections

This section focuses on exploring and learning the capabilities offered by the
`Reflection` library.

## PersistedAssemblies

A persisted assembly allows a dynamically created assembly at runtime to be
saved to disk, making it permanent.

```csharp
PersistedAssemblyBuilder assemblyBuilder = new PersistedAssemblyBuilder(
    new AssemblyName("AssemblyName"),
    typeof(object).Assembly
);
```

The difference from the code generated using `CSharpCompilation` (as we use) is
that it generates `IL` code directly without the need to be analyzed and
compiled into `IL` code. In this way, only Assembly conversion is required for
the generated code to work.
For an example usage, see
[`PersistedAssemblies`](./Reflection/PersistedAssemblies.cs).

Although `AssemblyBuilder` is more performant than `CSharpCompilation`, it is
more complex than `CSharpCompilation` in terms of typing. Therefore, we prefer
`CSharpCompilation` since the code generation operations we perform are in the
build phase.

## `TypeName`

TypeName returns a `TypeName` object that matches the name in the
`ReadOnlySpan<char>` parameter given by the `Parse` and `TryParse` methods.
This object is similar to `Type` and has many similar values like `Name`,
`FullName`, `AssemblyQualifiedName` etc.

```csharp
ReadOnlySpan<char> name = “ECMA-335 type name here...”;
if (TypeName.TryParse(name, out TypeName? parsed))
{
    ...
}
```

There is also an overload method where `Parse` and `TryParse` get an option.
This option is as follows.

```csharp
class TypeNameParseOptions
{
    public TypeNameParseOptions() { }
    public bool AllowFullyQualifiedName { get; set; } = true;
    public int MaxTotalComplexity { get; set; } = 10;
    public bool StrictValidation { get; set; } = false;
}
```

For examples of use, please
[`TypeNameParsing`](./Reflection/TypeNameParsing.cs) file.
