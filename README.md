# learn-dotnet

Learning new dotnet and csharp features

## Source Generator

We use the source generator to handle some routine tasks (such as creating
controls from classes under a specific namespace). In this way, we lighten
the workload.

We make the code to be generated either with the application model (schema
json) presented to us or by looking under certain namespaces according to the
content of the classes there.

### Usage

To use the generated code, we must reference the generated project to the
project we want to use the generate code for.

To see the example
[/source-generator/Domain/Domain.csproj](/source-generator/Domain/Domain.csproj)
see here.

### `ApplicationModel` Class

`ApplicationModel` is the class that represents the model where we keep the
information of the json schema or the code we will generate.

### `IIncrementalGenerator` Interface

We used the `IIncrementalGenerator` interface instead of the `ISourceGenerator`
interface because of its convenience and performance.

> :information_source:
>
> It's work with target framework `netstandard2.0` and
> `Microsoft.CodeAnalysis.CSharp 4.x` library.

### Using `Newtonsoft.Json` Library

We use the `Newtonsoft.Json` library to serialize and deserialise the
application model schema json provided to us.

> :information_source:
>
> In order for the `Newtonsoft.Json` library to work in harmony with the source
> generator, the settings as in
> [/source-generator/Domain/Domain.csproj](/source-generator/Domain/Domain.csproj)
> must be made.
