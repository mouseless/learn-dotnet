public class YearlySalary(SalaryCalculator _calculator)
    : Salary(_calculator)
{
    public override decimal GetSalary(Employee employee) =>
        base.GetSalary(employee) * 12;
}
