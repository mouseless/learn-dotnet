using CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<CollectionExpressions>();
serviceCollection.AddSingleton<DefaultLambdaParameters>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var collectionExpressions = serviceProvider.GetRequiredService<CollectionExpressions>();
var defaultLambdaParameters = serviceProvider.GetRequiredService<DefaultLambdaParameters>();

collectionExpressions.EmptyCollectionInitialization();
collectionExpressions.CollectionInitialization();
collectionExpressions.CallMethods();

defaultLambdaParameters.DefaultParameters();
