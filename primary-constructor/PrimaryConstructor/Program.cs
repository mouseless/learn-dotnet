using Microsoft.Extensions.DependencyInjection;
using PrimaryConstructor;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<IBookService, BookService>();
serviceCollection.AddSingleton<BookWorld>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dependent = serviceProvider.GetRequiredService<BookWorld>();

dependent.ShowMeCards();