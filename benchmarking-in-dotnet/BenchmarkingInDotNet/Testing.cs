using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkingInDotNet;

[SimpleJob(launchCount: 1, warmupCount: 10, iterationCount: 20)]
public class Testing
{
    [Benchmark]
    public string TestString()
    {
        var result = string.Empty;

        for (int i = 0; i < 100; i++)
        {
            result += i;
        }

        return result;
    }

    [Benchmark]
    public string TestStringBuilder()
    {
        var result = new StringBuilder();

        for (int i = 0; i < 100; i++)
        {
            result.Append(i);
        }

        return result.ToString();
    }
}
