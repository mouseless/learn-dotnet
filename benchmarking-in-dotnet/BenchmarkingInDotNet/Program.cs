// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using BenchmarkingInDotNet;

BenchmarkRunner.Run(assembly: typeof(Testing).Assembly);
//BenchmarkSwitcher.FromAssembly(typeof(Testing).Assembly).Run(args);
