using Microsoft.Extensions.DependencyInjection;
using TimeProvider.Service;

using SystemTimeProvider = System.TimeProvider;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(SystemTimeProvider.System);
serviceCollection.AddSingleton<MyService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var myService = serviceProvider.GetRequiredService<MyService>();

myService.IsMonday();
