# Benchmarking in DotNet

This is a simple project to demonstrate how we run benchmarksin dotnet using 
[BenchmarkDotNet](https://benchmarkdotnet.org/) library.

## BenchmarkDotNet

### Project Setup

- Create a console application, add `BenchmarkDotNet` nuget package.
- Add a test class which contains benchmark methods
- Add `[SimplJob]` attribute to the class
- Add `[Benchmark]` attrivbute to methods

> :warning:
>
>`BenchmarkDotNet` library only works with console applications

### Running Benchmark

Add following code to _program.cs_ file.

```csharp
  BenchmarkRunner.Run<Testing>();
```

See [How To Run](https://benchmarkdotnet.org/articles/guides/how-to-run.html)
for more details.

### Config

Configs provide setup for building a benchmark by setting up various 
configuration. See 
[Config](https://benchmarkdotnet.org/articles/configs/configs.html) for more 
details.

### Jobs

Jobs define how a run is performed based. Parameters for a benchmark run
such as _RunStrategy_,_RuntimeMoniker_,_LaunchCount_,_IterationCount_. See 
[Jobs](https://benchmarkdotnet.org/articles/configs/jobs.html) for mor details. 

### Setup and Cleanup

`BenchmarkDotNet` supports setup and cleanup methods before each launch or
invocation. See 
[Setup and Cleanup](https://benchmarkdotnet.org/articles/features/setup-and-cleanup.html)
for more details

### Important Notes

- Run benchmark tests in `RELEASE` configuration, `DEBUG` mode is not 
  recommended.
> :bulb:
>
> Enable optimize in your _.csproj_ file to run benchmarks in `DEBUG`
> configuration
> ```xml
>    <Optimize>true</Optimize>
> ```
- Static methods are not supported, instance methods can be tested
- Benchmarked classes should have `public`
- Benchmarked methods should be `public`
- Setup and Cleanup methods does not support Task return type


