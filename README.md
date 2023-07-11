# learn-dotnet

Learning new dotnet and csharp features

## Source Generator

We use the source generator to handle some routine tasks (such as creating
controls from classes under a specific namespace). In this way, we lighten
the workload.

We make the code to be generated either with the application model (schema
json) presented to us or by looking under certain namespaces according to the
content of the classes there.

> :information_source:
>
> We use the Newtonsoft.Json library to deserialise the application model
> schema json provided to us
>
> In order for the Newtonsoft.Json library to work compatible with the
> source generator, the following settings must be made.
>
> ```xml
> <ItemGroup>
>    <PackageReference Include="Newtonsoft.Json" Version="12.0.1" GeneratePathProperty="true" PrivateAssets="all" />
>  </ItemGroup>
>
>  <PropertyGroup>
>    <GetTargetPathDependsOn>$(GetTargetPathDependsOn);GetDependencyTargetPaths</GetTargetPathDependsOn>
>  </PropertyGroup>
>
>  <Target Name="GetDependencyTargetPaths">
>    <ItemGroup>
>      <TargetPathWithTargetPlatformMoniker Include="$(PKGNewtonsoft_Json)\lib\netstandard2.0\Newtonsoft.Json.dll" IncludeRuntimeDependency="false" />
>    </ItemGroup>
>  </Target>
> ```

### `IIncrementalGenerator` interface

We used the IIncrementalGenerator interface instead of the ISourceGenerator
interface because of its convenience and performance.

> :information_source:
>
> It's work with target framework `netstandard2.0` and
> `Microsoft.CodeAnalysis.CSharp 4.x` library.

### `Generator` Attribute

Elimideki json modelin controller code unu generate etmek için
`Microsoft.CodeAnalysis`'in `Generator` attribute unu kullanıyoruz.

### `ISourceGenerator` interface

`ISourceGenerator` implament ettiğimiz class build öncesinde analyzer çalıştırır.

> :information_source:
>
> class a `Generator` attribute verilmesi gerek

#### `Initialize()` method

...tbd

#### `Execute()` method

Bu method ile `GeneratorExecutionContext` nesnesine erişip elimizdeki
controller kodunu context teki `addSource()` ile source lara ekleyerek generate
edilmesini sağlıyoruz.

