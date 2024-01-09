using Microsoft.Extensions.DependencyInjection;
using PrimaryConstructor;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<Dependency>();
serviceCollection.AddSingleton<Dependent>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dependent = serviceProvider.GetRequiredService<Dependent>();

dependent.ShowMessage();
dependent.ShowBaseMessage();
dependent.ThrowException();