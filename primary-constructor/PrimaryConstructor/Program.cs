using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<SalaryCalculator>();
serviceCollection.AddSingleton<YearlySalary>();
serviceCollection.AddSingleton<MonthlySalary>();
serviceCollection.AddSingleton(TimeProvider.System);

var serviceProvider = serviceCollection.BuildServiceProvider();

var yearlySalary = serviceProvider.GetRequiredService<YearlySalary>();
var monthlySalary = serviceProvider.GetRequiredService<MonthlySalary>();

var employee = new Employee("John", new(2024, 01, 11));
Console.WriteLine($"{employee.Name}'s monthly salary: {monthlySalary.GetSalary(employee)}");
Console.WriteLine($"{employee.Name}'s yearly salary: {yearlySalary.GetSalary(employee)}");
