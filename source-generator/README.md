# Source Generator

We use the source generator to handle some routine tasks (such as creating
controllers from classes under a specific namespace). In this way, we lighten
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
> sequenceDiagram
>   Build Start->>+CodeGen: Build Starts
>   Domain Pre Build->>+CodeGen: Domain Run CodeGen
>   CodeGen->>+Domain Pre Build: Analyzer Generate Schema
>   Domain Pre Build->>+Domain Build: Go Next Stage
>   Domain Build->>+Domain Post Build: Go Next Stage
>   Domain Post Build->>+Cli: Domain Post Build Stage Runs Cli
>   Cli->>+Domain Post Build: Cli Convert Schema to Json
>   WebApp Pre Build->>+Cli: WebApp Pre Build Stage Runs Cli
>   Cli->>+WebApp Pre Build: Cli Copy Json File
>   WebApp Pre Build->>+CodeGen: Run Analyzer
>   CodeGen->>+WebApp Pre Build: Generate Controller from Json File
>   WebApp Pre Build->>+WebApp Build: Go Next Stage
> ```

> :information_source:
>
> It's work with target framework `netstandard2.0` and
> `Microsoft.CodeAnalysis.CSharp 4.x` library.

## Usage

To use the generated code, we must reference the generated project to the
project we want to use the generate code for.

To see the example
[/Domain/Domain.csproj](/Domain/Domain.csproj)
see here.

## `ServiceModel` Class

`ServiceModel` is the class that represents the model where we keep the
information of the json schema or the code we will generate.

## `IIncrementalGenerator` Interface

We used the `IIncrementalGenerator` interface instead of the `ISourceGenerator`
interface because of its convenience and performance.

By utilizing the `IIncrementalGenerator` interface, we are able to obtain the
syntax tree of the target project. From there, we can extract the semantic
model and retrieve the `ISymbol` using `GetDeclaredSymbol()`. With the
assistance of `ISymbol`, we can traverse the properties of namespaces, classes,
and their respective features within the target project. This approach allows
us to observe the classes residing under the target namespace and extract a
schema accordingly.

## Using `Newtonsoft.Json` Library

We use the `Newtonsoft.Json` library to serialize and deserialize the
service model schema json provided to us.

> :information_source:
>
> In order for the `Newtonsoft.Json` library to work during analyze, it must be
> added as `GeneratePathProperty="true" PrivateAssets="all"/>` and dll path
> must be given from dependencies. See
> [/CodeGen/CodeGen.csproj](/CodeGen/CodeGen.csproj) here for an example.

## Manage Build Stages

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
