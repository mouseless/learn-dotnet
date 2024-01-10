using Microsoft.Extensions.DependencyInjection;
using PrimaryConstructor;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<ICustomerService, CustomerService>();
serviceCollection.AddSingleton<Discount>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dependent = serviceProvider.GetRequiredService<Discount>();

dependent.Send();