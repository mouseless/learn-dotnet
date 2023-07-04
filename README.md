# learn-dotnet

Learning new dotnet and csharp features

## Source Generator

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
