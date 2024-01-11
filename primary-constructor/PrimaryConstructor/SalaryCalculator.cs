public class SalaryCalculator(TimeProvider _timeProvider)
{
    public decimal Calculate(DateTime dateOfHire)
    {
        var workedYear = _timeProvider.GetUtcNow().Year - dateOfHire.Year;
        if (workedYear < 0 || workedYear > 90)
        {
            throw new OutOfWorkingYearRangeException("An impossible working time was calculated.");
        }

        return workedYear switch
        {
            <= 5 => 10000,
            > 5 => 25000,
        };
    }
}
