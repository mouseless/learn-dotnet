using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace BenchmarkingInDotNet;

[AttributeUsage(AttributeTargets.Class)]
public class CustomJob : Attribute, IConfigSource
{
    public IConfig Config { get; }

    public CustomJob(int warmupCount = 5, int iterationCount = 5)
    {
        var job = new Job()
        {
            Run = { LaunchCount = 1, WarmupCount = warmupCount, IterationCount = iterationCount },
        };

        Config = ManualConfig.CreateEmpty().AddJob(job);
    }
}
