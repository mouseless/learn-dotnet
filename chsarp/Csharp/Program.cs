using Csharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<CollectionExpressions>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var collectionExpressions = serviceProvider.GetRequiredService<CollectionExpressions>();

collectionExpressions.EmptyCollectionInitialization();
collectionExpressions.CollectionInitialization();
collectionExpressions.CallMethods();
