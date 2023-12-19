# Benchmarking in DotNet

We use [BenchmarkDotNet](https://benchmarkdotnet.org/) library for benchmarking

## BenchmarkDotNet

### Project Setup

Create a console application, add 

`BenchmarkDotNet` library only works with console applications
### Running Benchmark

There are two options for running benchmarks. Add following codes to
_program.cs_ file.

#### BenchmarkRunner

Starts the benchmark on given type or assembly.

```csharp
  BenchmarkRunner.Run<Testing>();
```

```csharp
  BenchmarkRunner.Run(typeof(Testing).Assembly);
```

#### BenchmarkSwitcher

Starts bencmark with given arguements from cli

```csharp
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
 ```

```
dotnet run -c Release -- --job short --runtimes net472 net7.0 --filter *Testing*
```
### Jobs

Jobs define how benchmarking is performed. Every benchmark is 

### Config

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


