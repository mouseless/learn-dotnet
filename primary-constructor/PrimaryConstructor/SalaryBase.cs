public abstract class SalaryBase(SalaryCalculator _calculator)
{
    public abstract decimal GetSalary(Employee employee);

    protected decimal Calculate(DateTime dateOfHire) =>
        _calculator.Calculate(dateOfHire);
}
