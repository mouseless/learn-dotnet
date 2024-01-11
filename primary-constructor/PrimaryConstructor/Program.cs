using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<SalaryCalculator>();
serviceCollection.AddSingleton<YearlySalary>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dependent = serviceProvider.GetRequiredService<YearlySalary>();

var employee = new Employee("John", new(2024, 01, 11));
Console.WriteLine($"{employee.Name}'s yearly salary: {dependent.GetSalary(employee)}");
