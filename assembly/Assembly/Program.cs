using LearnAssembly;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<PersistedAssemblies>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var persistedAssembly = serviceProvider.GetRequiredService<PersistedAssemblies>();

persistedAssembly.Run();