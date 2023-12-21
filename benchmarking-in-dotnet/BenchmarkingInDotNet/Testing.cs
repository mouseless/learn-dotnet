using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkingInDotNet;

[SimpleJob(warmupCount: 1, iterationCount: 2, invocationCount: 10)]
public class Testing
{
    [ParamsSource(nameof(Values))]
    public int Count { get; set; }

    public IEnumerable<int> Values => [10, 20];

    [GlobalSetup]
    public void GlobalSetup() =>
        Console.WriteLine("Global Setup");

    [GlobalCleanup]
    public void GlobalCleanup() =>
        Console.WriteLine("Global Cleanup");

    [Benchmark(Baseline = true)]
    public string TestString()
    {
        var result = string.Empty;

        for (int i = 0; i < Count; i++)
        {
            result += i;
        }

        return result;
    }

    [Benchmark]
    public string TestStringBuilder()
    {
        var result = new StringBuilder();

        for (int i = 0; i < Count; i++)
        {
            result.Append(i);
        }

        return result.ToString();
    }
}
