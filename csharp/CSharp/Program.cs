using CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<CollectionExpressions>();
serviceCollection.AddSingleton<LambdaParameters>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var @params = serviceProvider.GetRequiredService<Params>();
var collectionExpressions = serviceProvider.GetRequiredService<CollectionExpressions>();
var lambdaParameters = serviceProvider.GetRequiredService<LambdaParameters>();

@params.Use();

collectionExpressions.EmptyCollectionInitialization();
collectionExpressions.CollectionInitialization();
collectionExpressions.CallMethods();

lambdaParameters.OptionalParameters();
lambdaParameters.ParamsArrayParameters();
lambdaParameters.NewAcceptedBehavior();