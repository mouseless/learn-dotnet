# Reflections

This project was created to deepen our understanding of reflection and learn how
to use it more effectively. Our goal is to explore the principles of how
reflection works, delve into advanced topics like dynamic type inspection,
method invocation, and runtime metadata manipulation, and experiment with newly
introduced features in practice.

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

`TypeName` is used to parse and analyze type names in `.NET`. It operates at
build time. It takes `ReadOnlySpan<char>` and parses into `Type` using `Parse`
or `TryParse` as shown below:

```csharp
ReadOnlySpan<char> name = "ECMA-335 type name here...";
if (TypeName.TryParse(name, out TypeName? parsed))
{
    ...
}
```

> :information:
>
> Recommended use cases are in areas like NativeAOT and SourceGenerator.

For usage examples, please refer to the
[`TypeNameParsing`](./Reflection/TypeNameParsing.cs) file.