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

personService.AddPerson();
personService.AddPerson("Mike", "Hamilton");
personService.AddPerson("Brian", "Max");

personService.UpdatePerson("Brian", "Michael");

personService.DeletePerson("Mike");

foreach (var person in personService.AllPersons())
{
    Console.WriteLine($"Name:{person.Name}, MiddleName: {person.MiddleName}, InitialName: {person.InitialName}");
}
