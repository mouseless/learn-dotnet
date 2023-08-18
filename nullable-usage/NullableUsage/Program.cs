using Microsoft.Extensions.DependencyInjection;
using NullableUsage;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(typeof(IFinder), (sp) => sp.GetRequiredService<Persons>());
serviceCollection.AddSingleton<PersonService>();
serviceCollection.AddSingleton<Persons>();
serviceCollection.AddSingleton(typeof(Func<Person>), (sp) => () => sp.GetRequiredService<Person>());
serviceCollection.AddTransient<Person>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var personService = serviceProvider.GetRequiredService<PersonService>();

Console.WriteLine("Hello world");
