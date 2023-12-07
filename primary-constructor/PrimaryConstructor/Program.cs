using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimaryConstructor;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<LogSystem>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var logSystem = serviceProvider.GetRequiredService<LogSystem>();

logSystem.TestLog("Test Logging");
