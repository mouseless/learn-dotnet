# Native AOT

`<PublishAot>true</PublishAot>` uygulamayı aot olarak yayınlamak için
`<IsAotCompatible>true</IsAotCompatible>` is used to indicate whether a library
is compatible with Native AOT

- Not necessarily faster at runtime, but no longer uses JIT so can be.
- Publis is slower
- Startup time is significantly faster
- Takes less space, better for docker
- Loosing some capabilities due to not using JIT and trimming

## Ne işe yarıyor?

you through building native libraries that can be consumed by other programming
languages with NativeAOT.

### Hangi projerlerde kullanıyor? Console? Web?

web - yes
console - yes
lib - yes

#### Web detaylı +,-

bir yerde https desteği yok gibi okudum detaylı bakılacak :x:

## Bu feature neden geliştirildi ?

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

## Nasıl çalıştırılır ?

### Geliştirme aşamasında nasıl olacak?

if PublishAot is present in the project file, the behavior should be the same
between CoreCLR(default development) and Native AOT.

## Dockerda problem olur mu?

dotnet in sample projede örneği var. Sorun olmayacaktır.
bkz: https://github.com/dotnet/samples/blob/main/core/nativeaot/HelloWorld/README.md

Native AOT publish edilen platform'u targetladığı ve sadece o platformda
çalışabildiği için Docker üzerinden container'da çalıştırmak farklı platformdan
dolayı çalışmama problemini ortadan kaldıracağı için öneriliyor.

## Artıları neler?

### Build native libraries

Publishing .NET class libraries as Native AOT allows creating libraries that can
be consumed from non-.NET programming languages. The produced native library is
self-contained and doesn't require a .NET runtime to be installed.
bkz: https://github.com/dotnet/samples/tree/main/core/nativeaot/NativeLibrary

## Do da kullanabilir miyiz ? Blueprint olarak sunabilirmiyiz ?

### csproj da publish aot diye belirttiğimizi gördük. Do ile do yu kullananlara bunu nasıl sunacağız. Do da aot olarak isteğe bağlı nasıl yayınanabilir ?

## Her package ile çalışır mı? çalışmıyorsa hangi tarz packageler ile uygun değil?

### package.props ve build.props kullanıyoruz aynı solutionda diğer projeler ile ortak alanda bulunması sorun teşkil ediyor mu ?

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
