# learn-dotnet

Learning new dotnet and csharp features

## Source Generator

### `Generator` Attribute

Elimideki json modelin controller code unu generate etmek için
`Microsoft.CodeAnalysis`'in `Generator` attribute unu kullanıyoruz.

> :information_source:
>
> Serialize ve deserialize için `System.Text.Json`, `Newtonsoft.Json` vb.
> kütüphaneleri kullanıldığında sorun yaratabiliyor bunun için aşağıdaki
> configleri yapmak gerekiyor.
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

### `IIncrementalGenerator` interface

`ISourceGenerator` aynı işi yapmaktadır. ASP.NET Core 6.0 ve sonraki sürümler için
`ISourceGenerator`dan daha performanslı ve optimize code yazmayı sağlar.

> :information_source:
>
> Microsoft.CodeAnalysis.CSharp 4.x ile çalışır.

#### `Initialize()` method

Generate öncesi hazırlıkları yaptığımız method.
`IncrementalGeneratorInitializationContext`parametresini alır.

#### `IncrementalGeneratorInitializationContext` nesnesi

Initialize methodundan erişilebilir.

- Çalıştığı projenin veya solition bilgilerine erişilebilir
- Config dosyasındaki bilgilere erişilebilir
- RegisterForSyntaxNotifications ile izlenen dosyalarda veya projelerde yapılan değişikliklere tepki verebilir.
- Generator'ın başlatma işlemlerini özelleştirme, bağımlılıklara erişme,
  değişiklikleri izleme ve önbellekleme gibi çeşitli görevleri yerine getirmek için kullanılabilir.
