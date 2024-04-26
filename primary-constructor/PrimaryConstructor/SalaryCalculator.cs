public class SalaryCalculator(TimeProvider _timeProvider)
{
    public decimal Calculate(DateTime dateOfHire)
    {
        var workedYear = _timeProvider.GetUtcNow().Year - dateOfHire.Year;
        if (workedYear is < 0 or > 90)
        {
            throw new OutOfWorkingYearRangeException("An impossible working time was calculated.");
        }

        return workedYear switch
        {
            <= 5 => 10000,
            > 5 => 25000,
            _ => throw new NotImplementedException(),
        };
    }
}