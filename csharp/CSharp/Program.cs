using CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<CollectionExpressions>();
serviceCollection.AddSingleton<EncodingDecoding>();
serviceCollection.AddSingleton<LambdaParameters>();
serviceCollection.AddSingleton<Params>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var collectionExpressions = serviceProvider.GetRequiredService<CollectionExpressions>();
var encodingDecoding = serviceProvider.GetRequiredService<EncodingDecoding>();
var lambdaParameters = serviceProvider.GetRequiredService<LambdaParameters>();
var @params = serviceProvider.GetRequiredService<Params>();

@params.Use();

collectionExpressions.EmptyCollectionInitialization();
collectionExpressions.CollectionInitialization();
collectionExpressions.CallMethods();

lambdaParameters.OptionalParameters();
lambdaParameters.ParamsArrayParameters();
lambdaParameters.NewAcceptedBehavior();

encodingDecoding.RunShowCases();