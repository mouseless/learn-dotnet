using LearnReflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<TypeNameParsing>();
serviceCollection.AddSingleton<PersistedAssemblies>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var typeNameParsing = serviceProvider.GetRequiredService<TypeNameParsing>();
var persistedAssembly = serviceProvider.GetRequiredService<PersistedAssemblies>();

typeNameParsing.IsThisTypeOk(typeof(string));
typeNameParsing.IsThisTypeOk(typeof(int));
typeNameParsing.IsThisTypeOk(typeof(double));
typeNameParsing.IsThisTypeOk(typeof(List<int>));

persistedAssembly.Run();