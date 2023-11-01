# Net 7

## .NET regular expression source generators

```csharp
private static readonly Regex s_abcOrDefGeneratedRegex =
new(pattern: "abc|def",
    options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

private static void EvaluateText(string text)
{
    if (s_abcOrDefGeneratedRegex.IsMatch(text))
    {
        // Take action with matching text
    }
}
// You can now rewrite the previous code as follows:

[GeneratedRegex("abc|def", RegexOptions.IgnoreCase, "en-US")]
private static partial Regex AbcOrDefGeneratedRegex();

private static void EvaluateText(string text)
{
    if (AbcOrDefGeneratedRegex().IsMatch(text))
    {
        // Take action with matching text
    }
}
```

## Generic math

```csharp
static T Add<T>(T left, T right)
    where T : INumber<T>
{
    return left + right;
}
```

## Central Package Management

To use it, add `Directory.Packages.props` file. In this file, set the `MSBuild`
property `ManagePackageVersionsCentrally` to `true` and add versions for common
package dependency using `PackageVersion` items.

Inside, you then define each of the respective package versions required of
your projects using `<PackageVersion />` elements that define the package `ID`
and version.

[Directory.Packages.props]
```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Newtonsoft.Json" Version="13.0.1" />
  </ItemGroup>
</Project>
```

For each project, you then define a `<PackageReference />` but omit the
`Version` attribute since the version will be attained from a corresponding
`<PackageVersion />` item.

[.csproj]
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" />
  </ItemGroup>
</Project>
```

If you had multiple `Directory.Packages.props` files in your repository then
read [this](https://learn.microsoft.com/en-us/nuget/consume-packages/Central-Package-Management#central-package-management-rules).

## dotnet workload command

## [Source generation for platform invokes](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke-source-generation)

## [Customize a Json Contract](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts)

## open telemetry ile proje metriclerini takip etmeyi sağlıyor

## Breaking Changes

- Returning a collectible `Assembly` in the
 `AssemblyLoadContext.Load(AssemblyName)` override or the
 `AssemblyLoadContext.Resolving` event of a non-collectible
 `AssemblyLoadContext` throws a `System.IO.FileLoadException`
 with a `NotSupportedException` as the inner exception.
- A FormatException is thrown if the precision is greater than 999,999,999.
  This change was implemented in the parsing logic that affects all numeric
  types.
- ContentRootPath for apps launched by Windows Shell
  Host.CreateDefaultBuilder no longer defaults the ContentRootPath property to
  the current directory if it's the System special folder on Windows. Instead,
  the base directory of the application is used.
- Environment variable prefixes

  ```csharp
  //// old
  const string myValue = "value1";
  Environment.SetEnvironmentVariable("MY_PREFIX__ConfigKey", myValue);

  IConfiguration config = new ConfigurationBuilder()
      .AddEnvironmentVariables(prefix: "MY_PREFIX:")
      .Build();

  //// new
  const string myValue = "value1";
  Environment.SetEnvironmentVariable("MY_PREFIX__ConfigKey", myValue);

  IConfiguration config = new ConfigurationBuilder()
      .AddEnvironmentVariables(prefix: "MY_PREFIX__")
      .Build();
  ```

- Solution-level --output option no longer valid for build-related commands
  The dotnet CLI will error if the --output/-o option is used with a solution
  file. Starting in the 7.0.201 SDK, a warning will be emitted instead, and in
  the case of dotnet pack no warning or error will be generated.
- SDK no longer calls ResolvePackageDependencies
  Package information is added from `PreprocessPackageDependenciesDesignTime`
  into the design-time build cache. If you depended on `PackageDependencies`
  and `PackageDefinitions` in your build, you'll see build errors such as `No
  dependencies found`.
