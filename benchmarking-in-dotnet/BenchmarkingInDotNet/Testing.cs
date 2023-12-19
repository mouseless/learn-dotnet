using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkingInDotNet;

[CustomJob(5, 10)]
public class Testing
{
    [ParamsSource(nameof(Values))]
    public int Count { get; set; }

    // public property
    public IEnumerable<int> Values => new[] { 100, 200 };

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
