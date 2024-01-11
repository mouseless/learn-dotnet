public class Salary(SalaryCalculator _calculator)
{
    public virtual decimal GetSalary(Employee employee) =>
        _calculator.Calculate(employee.Seniority);
}
