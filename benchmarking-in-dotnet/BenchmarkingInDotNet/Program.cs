using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkingInDotNet;

BenchmarkRunner.Run<Testing>(config: DefaultConfig.Instance.WithArtifactsPath(@"./.benchmark"));