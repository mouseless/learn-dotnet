# Benchmarking in DotNet

This is a simple project to demonstrate how we run benchmarking dotnet using
[BenchmarkDotNet][] library.

## BenchmarkDotNet

### Project Setup

- Create a console application, add `BenchmarkDotNet` nuget package.
- Add a test class which contains benchmark methods
- Add `[SimplJob]` attribute to the class
- Add `[Benchmark]` attribute to methods

> [!WARNING]
>
>`BenchmarkDotNet` library only works with console applications

### Running Benchmark

Add following code to _program.cs_ file.

```csharp
  BenchmarkRunner.Run<Testing>();
```

See [How To Run][] for more details.

### Config

Configs provide setup for building a benchmark by setting up various
configuration. See [Config][] for more details.

### Jobs

Jobs define how a run is performed based. Parameters for a benchmark run
such as _RunStrategy_,_RuntimeMoniker_,_LaunchCount_,_IterationCount_. See
[Jobs][] for more details.

### Setup and Cleanup

`BenchmarkDotNet` supports setup and cleanup methods before each launch or
invocation. See [Setup and Cleanup][] for more details

### Important Notes

- Run benchmark tests in `RELEASE` configuration, `DEBUG` mode is not
  recommended.

> [!TIP]
>
> Enable optimize in your _.csproj_ file to run benchmarks in `DEBUG`
> configuration
> ```xml
>    <Optimize>true</Optimize>
> ```

- Static methods are not supported, instance methods can be tested
- Benchmarked classes should have `public`
- Benchmarked methods should be `public`
- Setup and Cleanup methods does not support `Task` return type
- We use `./.benchmark/` path for output results, add this directory to
  `.gitignore` file

[BenchmarkDotNet]: https://benchmarkdotnet.org/
[How To Run]: https://benchmarkdotnet.org/articles/guides/how-to-run.html
[Config]: https://benchmarkdotnet.org/articles/configs/configs.html
[Jobs]: https://benchmarkdotnet.org/articles/configs/jobs.html
[Setup and Cleanup]: https://benchmarkdotnet.org/articles/features/setup-and-cleanup.html