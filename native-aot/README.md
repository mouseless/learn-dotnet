# Native AOT

- `<PublishAot>true</PublishAot>` uygulamayı aot olarak yayınlamak için
- `<IsAotCompatible>true</IsAotCompatible>` is used to indicate whether a library
  is compatible with Native AOT
  This one property will enable the three underlying Roslyn analyzers:

  - EnableTrimAnalyzer
  - EnableSingleFileAnalyzer
  - EnableAotAnalyzer

## Ne işe yarıyor?

you through building native libraries that can be consumed by other programming
languages with NativeAOT.

### Hangi projerlerde kullanıyor? Console? Web?

web - yes
console - yes
lib - yes

#### ASP.NET Core Web API (native AOT) detaylı +,-

ASP.NET Core Web API'si (yerel AOT) template'i olabilecek en hafif program ile geliyor
CreateSlimBuilder kullanılıyor bu durumda IIS, https gibi destekleri olmuyor.
bunlar daha sonra aktif edilebilir.

## Bu feature neden geliştirildi ?

minimal api ler için gereksiz yükü azaltarak ahead of time da yapılabilecek işi
just in time da yaptırıp startup süresini ve gereksiz bellek boyutunu azalmak
için geliştirilmiş.

## Eksileri neler? Bize nerelerde sorun çıkarabilir ?

MVC, Blazor, SPA, EntityF, JWT hariç diğer kimlik doğrulaması, Oturum, Spa desteklenmiyor.

- No dynamic loading, for example, Assembly.LoadFile.
- No run-time code generation, for example, System.Reflection.Emit.
- No C++/CLI.
- Windows: No built-in COM.
- Requires trimming
- Implies compilation into a single file, which has known incompatibilities
- Apps include required runtime libraries, increasing their size as compared to
  framework-dependent apps.
- System.Linq.Expressions always use their interpreted form, which is slower
  than run-time generated compiled code.
- Not all the runtime libraries are fully annotated to be Native AOT compatible.
  That is, some warnings in the runtime libraries aren't actionable by end
  developers.

## Artıları neler?

- Not necessarily faster at runtime, but no longer uses JIT so can be.
- Publis is slower
- Startup time is significantly faster
- Takes less space, better for docker
- Loosing some capabilities due to not using JIT and trimming

### Build native libraries

Publishing .NET class libraries as Native AOT allows creating libraries that can
be consumed from non-.NET programming languages. The produced native library is
self-contained and doesn't require a .NET runtime to be installed.
bkz: https://github.com/dotnet/samples/tree/main/core/nativeaot/NativeLibrary

## Nasıl çalıştırılır ?

`PublishAot` enable edip publish alındığında çalışıyor. publish alırken console
da `Generating native code` yazısını gördüğünüzde aot okay demektir ve çıktı
4 file oluyor.

### Geliştirme aşamasında nasıl olacak?

if PublishAot is present in the project file, the behavior should be the same
between CoreCLR(default development) and Native AOT.

## Dockerda problem olur mu?

dotnet in sample projede örneği var. Sorun olmayacaktır.
bkz: https://github.com/dotnet/samples/blob/main/core/nativeaot/HelloWorld/README.md

Native AOT publish edilen platform'u targetladığı ve sadece o platformda
çalışabildiği için Docker üzerinden container'da çalıştırmak farklı platformdan
dolayı çalışmama problemini ortadan kaldıracağı için öneriliyor.

## Do da kullanabilir miyiz ? Blueprint olarak sunabilirmiyiz ?

kullanmak basit PublishAot yi enable edince aot olarak publish ediyorsun ve eğer
bir sorun varsa ve aot ile publish olmuyorsa errorleri gösteriyor ancak yinede
errorleri çözünce sorunların hepsinin kalktığını düşünmemek gerek ve full test
etmekte fayda var deniyor.

!! sanırım newtonsoft json sanırım aot desteklemiyor. ayrıca buradaki kaynak
var olan projenin aot geçişini anlatıyor
https://blog.martincostello.com/native-aot-make-dotnet-lambda-go-brr/#:~:text=involved%20proposition%20though...-,Removing%20Newtonsoft.Json,-The%20Alexa.NET

### csproj da publish aot diye belirttiğimizi gördük. Do ile do yu kullananlara bunu nasıl sunacağız. Do da aot olarak isteğe bağlı nasıl yayınanabilir ?

The only aproach I could find was maintaining Native AOT compatible versions of
`Do` libraries for example `Do.Blueprints.Service.AOT`, add `PublishAot` to
their csproj. Using this approach Do packages with `AOT` suffix will be Native
AOT compatible thus wont create errors when the user publishes using Native AOT.
Target platform may cause issues, when publishing using Native AOT it targets
the platform it is published on so the `.dll` and `.exe` can only work on the
same platform it is published for example `linux, windows x32, windows x64)` etc

## Her package ile çalışır mı? çalışmıyorsa hangi tarz packageler ile uygun değil?

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0

Native AOT is not compatible with all packages. There are analyzers that show
possible errors before publish and you can determine which packages are not
compatible by test publishing and getting the error log.

### Catching possible errors due to trimming

https://devblogs.microsoft.com/dotnet/creating-aot-compatible-libraries/#analyzing-net-libraries
You can enable analyzers to determine errors before Native AOT publish. Roslyn
Analyzers does not guarantee catchin all errors but shows the errors fast.
Publishing using Native AOT is another way to find errors, takes longer, can't
show errors on IDE like Roslyn Analyzers but catches all the errors.

### Error attributes

- RequiresUnreferencedCode: Current type or method is not compatible with
  trimming
- RequiresDynamicCode: If the method explicitly calls into
  System.Reflection.Emit which isnt compatible with Native AOT
- DynamicallyAccessedMembers: When using generic `T` to create a new type
  instance from, trimming tool doesn't know which types are going to be used
  when calling the constructor and can't guarantee it wont trim a necessary
  constructor for the app to work.

### package.props ve build.props kullanıyoruz aynı solutionda diğer projeler ile ortak alanda bulunması sorun teşkil ediyor mu ?

Sorun olmuyor. `PublishAot`'yi `directory.build.props`'a ekleyebilirsiniz
deniyor.

## source generator ile uyumlu mu ?

Native AOT is compatible with source generation, it is not compatible with
runtime code generation.

## InvariantGlobalization nedir ?

`<InvariantGlobalization>true</InvariantGlobalization>` enables you to remove
application dependencies on globalization data and globalization behavior. This
mode is an opt-in feature that provides more flexibility if you care more about
reducing dependencies and the size of distribution than globalization
functionality or globalization-correctness.

The following scenarios are affected when the invariant mode is enabled. Their
invariant mode behavior is defined in this document.

- Cultures and culture data
- String casing
- String sorting and searching
- Sort keys
- String Normalization
- Internationalized Domain Names (IDN) support
- Time Zone display name on Linux

for more https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md

## Publishing Options

- `PublishSelfContained`: Standalone application the does not need a framework
- `PublishTrimmed`: https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained
- `PublishReadyToRun`: Pre-Compiles the application, does not use JIT at start
  up (does not remove JIT, you still get the benefits from it)
- `PublishSingleFile`
- `NativeAot`

### More Publish Options

- `OptimizationPrefference`: size/speed. Optimize size or speed, default value
  is size
- `InvatiantGlobalization`: [InvatiantGlobalization](#invariantglobalization-nedir)
- `EventSourceSupport`: true/false. Default for Native AOT
- `ServerGarbageCollection`: true/false
- `GarbageCollectionAdaptationMode`: 0/1. Default for Native AOT

## reflection a izin yoksa DI çalışmıyor mu ?

M.E.DI, Autofac, DryIoc, Grace, LightInject, StrongInject and other DI
containers working.

reflection-free mode ta kullanırsak bazı reflectionlara izin veriliyor. ama ms bunu önermiyor.

https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/reflection-free-mode.md

## Controller lar çalışıyormu ?

hayır mvc desteklenmediği için çalışmıyor. AddController() service i native aot
de hata veriyor.

## RD.xml dosyası nedir ?

AOT compiler might miss some types if the application uses reflection, you can
add an `rd.xml` file to help ILCompiler find the types that should be analyzed.
You can tell to use the probided `rd.xlm` file by adding
`RdXmlFile Include="rd.xml"` to your project file. To make compiler analyze all
the types in the module add `Assembly Name="AssemblyName" Dynamic="RequiredAll"`
You can also select some types to be analyzed by adding `Types` tag under the
`Assembly`, `<Type Name="TypeDirective" Dynamic="Required All" />` which will
make ILCompiler analyze all occurences of the given type. You can check the type
directives from;
https://github.com/dotnet/corert/blob/master/Documentation/using-corert/rd-xml-format.md#types-directives

## Neden tekrar eklenebilen bazı featurelar kaldırıldı örneğin https gibi ?

Most of the remaining features that were removed have been taken out because
they're not typically used in the scenarios that AOT targets i.e.
cloud/serverless, Linux, environments:

- These environments tend to be Linux (if you're worried about startup times,
  you're not going to be hosting behind IIS on Windows) so removing the EventLog
  and IIS support makes sense.
- It's common to handle HTTPS or HTTP/3 behind a TLS termination proxy, so that
  you don't have to manage certificates in your app.
- You won't (hopefully!) have a debugger attached in a production environment.
- UseStaticWebAssets() isn't necessary when publishing your app, and typically
  isn't used by API applications anyway

https://andrewlock.net/exploring-the-dotnet-8-preview-comparing-createbuilder-to-the-new-createslimbuilder-method/
