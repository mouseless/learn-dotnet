# Assembly

This project was created to deepen our understanding of assemblies and learn how
to use them more effectively. Our goal is to explore the principles of how
assemblies work, delve into advanced topics like dynamic creation and
management, and experiment with newly introduced features in practice.

## PersistedAssemblies

A persisted assembly allows a dynamically created assembly at runtime to be
saved to disk, making it permanent. This feature enables the generated code or
types, which were previously only temporary and stored in memory, to be saved as
a file for future use. For an example usage, see
[`PersistedAssemblies`](./Assembly/PersistedAssemblies.cs). Before this feature
was introduced with `.NET 9`, assemblies were only kept in memory, and writing
them to disk required additional effort. Persisted Assembly eliminates this
overhead, making the process much simpler and more efficient.
