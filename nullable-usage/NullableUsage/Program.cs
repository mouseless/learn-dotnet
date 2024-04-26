using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NullableUsage;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(typeof(IFinder), (sp) => sp.GetRequiredService<Persons>());
serviceCollection.AddSingleton<PersonService>();
serviceCollection.AddSingleton<Persons>();
serviceCollection.AddSingleton(typeof(Func<Person>), (sp) => () => sp.GetRequiredService<Person>());
serviceCollection.AddTransient<Person>();
serviceCollection.AddSingleton<Json>();

serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var personService = serviceProvider.GetRequiredService<PersonService>();

personService.AddPerson();
personService.AddPerson("Mike", "Hamilton");
personService.AddPerson("Brian", "Max");

personService.UpdatePerson("Brian", "Michael");

personService.DeletePerson("Mikey");

personService.DisplayPeople();