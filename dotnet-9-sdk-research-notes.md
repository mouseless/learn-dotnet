# .NET 9 SDK Research Notes

## Unit Testing

### Run tests in parallel

In `.NET 9`, dotnet test is more fully integrated with MSBuild. Because MSBuild
supports building in parallel, you can run tests for the same project across
different target frameworks in parallel. By default, MSBuild limits the number
of parallel processes to the number of processors on the computer. You can also
set your own limit using the `-maxcpucount` switch. If you want to opt out of
the parallelism, set the `TestTfmsInParallel` MSBuild property to `false`.

### Terminal logger test display

Test result reporting for dotnet test is now supported directly in the MSBuild
terminal logger. For better result.

## .NET tool roll-forward

A new option for dotnet tool install lets users decide how .NET tools should be
run. When you install a tool via dotnet tool install, or when you run tool via
dotnet tool run `<toolname>`, you can specify a new flag called
`--allow-roll-forward`.

## Terminal logger

Enabled by default, specify the `--tl:off` command-line option to disable
terminal logger for a specific command. Or, to disable terminal logger more
broadly, set the `MSBUILDTERMINALLOGGER` environment variable to `off`.

## Faster NuGet dependency resolution for large repos

The NuGet dependency resolver has been overhauled to improve performance and
scalability for all `<PackageReference>` projects.

If you encounter any issues, such as restore failures or unexpected package
versions, you can revert.

## MSBuild script analyzers ("BuildChecks")

.NET 9 introduces a feature that helps guard against defects and regressions in
your build scripts. To run the build checks, add the `/check` flag to any
command that invokes MSBuild. For example, `dotnet build myapp.sln /check`
builds the myapp solution and runs all configured build checks.

## Analyzer mismatch

"CSC : warning CS9057: The analyzer assembly '..\dotnet\sdk\8.0.200\Sdks\Microsoft.NET.Sdk.Razor\source-generators\Microsoft.CodeAnalysis.Razor.Compiler.SourceGenerators.dll' references version '4.9.0.0' of the compiler, which is newer than the currently running version '4.8.0.0'."

When this error is received, .net 9 can now create a solution itself and
download this package `Microsoft.Net.Sdk.Compilers.Toolset`.

## Containers

### Publishing support for insecure registries

Starting with .NET 9, the SDK introduces support for working with insecure
container registries, enabling direct publishing of container images to
registries without HTTPS or with invalid certificates (e.g., for testing or
local development). In previous versions, the SDK required registries to support
HTTPS and have valid TLS certificates.

### Environment variable naming

Environment variables that the container publish tooling uses to control some
of the finer aspects of registry communication and security now start with the
prefix `DOTNET` instead of `SDK`. The `SDK` prefix will continue to be supported
in the near term.
