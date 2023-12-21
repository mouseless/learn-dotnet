using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkingInDotNet;

BenchmarkRunner.Run(assembly: typeof(Testing).Assembly, config: DefaultConfig.Instance.WithArtifactsPath(@"./.benchmark"));

