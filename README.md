# learn-dotnet

Learning new dotnet and csharp features

## Source Generator

We use the source generator to handle some routine tasks (such as creating
controls from classes under a specific namespace). In this way, we lighten
the workload.

We make the code to be generated either with the service model (schema
json) presented to us or by looking under certain namespaces according to the
content of the classes there.

> :information_source:
>
> The purpose of this source generator learning is to automatically create
> controllers by inspecting the classes within the target namespace in the
> `Domain` project. This approach saves people from the burden of writing
> new controllers for `WebApp` project whenever a new class is added to the
> `Domain`.
>
> ```mermaid
> graph TD
>     D[Domain] -->|Generate schema json in .cs file| C[CodeGen]
>     W[Web App] -->|Generate Controller| C
>     D -->|Extract Schema in .cs file as Json| Cl[Cli]
>     W -->|Copy json file from Domain| Cl
> ```

### Usage

To use the generated code, we must reference the generated project to the
project we want to use the generate code for.

To see the example
[/source-generator/Domain/Domain.csproj](/source-generator/Domain/Domain.csproj)
see here.

### `ServiceModel` Class

`ServiceModel` is the class that represents the model where we keep the
information of the json schema or the code we will generate.

### `IIncrementalGenerator` Interface

We used the `IIncrementalGenerator` interface instead of the `ISourceGenerator`
interface because of its convenience and performance.

By utilizing the `IIncrementalGenerator` interface, we are able to obtain the
syntax tree of the target project. From there, we can extract the semantic
model and retrieve the `ISymbol` using `GetDeclaredSymbol()`. With the
assistance of `ISymbol`, we can traverse the properties of namespaces, classes,
and their respective features within the target project. This approach allows
us to observe the classes residing under the target namespace and extract a
schema accordingly.

> :information_source:
>
> It's work with target framework `netstandard2.0` and
> `Microsoft.CodeAnalysis.CSharp 4.x` library.

### Using `Newtonsoft.Json` Library

We use the `Newtonsoft.Json` library to serialize and deserialise the
service model schema json provided to us.

## Build Stages

Build stages are used to perform operations during, before, or after the build
process. You can refer to the example in the
[/source-generator/Domain/Domain.csproj](/source-generator/Domain/Domain.csproj)
or
[/source-generator/WebApp/WebApp.csproj](/source-generator/WebApp/WebApp.csproj)
file for an illustration.

### After Build

In this stage, we use script files to perform operations on the generated code.

An example of transforming a generated file into another format can be found
in the
[/source-generator/Domain/Domain.csproj](/source-generator/Domain/Domain.csproj)
file.

### Before Core Compile

This stage is used to execute script files that add external resources to the
project before compilation.

For example, please refer to
[/source-generator/WebApp/WebApp.csproj](/source-generator/WebApp/WebApp.csproj)
for an example script that performs the task of copying resources between two
projects during Before Core Compile.
