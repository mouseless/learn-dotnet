# Reflections

This section focuses on exploring and learning the capabilities offered by the
`Reflection` library.

## PersistedAssemblies

A persisted assembly allows a dynamically created assembly at runtime to be
saved to disk, making it permanent. This feature enables the generated code or
types, which were previously only temporary and stored in memory, to be saved as
a file for future use. For an example usage, see
[`PersistedAssemblies`](./Reflection/PersistedAssemblies.cs). Before this feature
was introduced with `.NET 9`, assemblies were only kept in memory, and writing
them to disk required additional effort. Persisted Assembly eliminates this
overhead, making the process much simpler and more efficient.

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
