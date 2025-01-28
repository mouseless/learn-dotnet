using LearnReflection;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<TypeNameParsing>();
serviceCollection.AddSingleton<PersistedAssemblies>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var typeNameParsing = serviceProvider.GetRequiredService<TypeNameParsing>();
var persistedAssembly = serviceProvider.GetRequiredService<PersistedAssemblies>();

typeNameParsing.Guess();

persistedAssembly.Run();