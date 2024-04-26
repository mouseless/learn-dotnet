public class MonthlySalary(SalaryCalculator _calculator)
    : SalaryBase(_calculator)
{
    public override decimal GetSalary(Employee employee) =>
        Calculate(employee.DateOfHire);
}