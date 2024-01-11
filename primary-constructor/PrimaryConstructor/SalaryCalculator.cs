public class SalaryCalculator
{
    public decimal Calculate(string seniority) =>
        seniority switch {
            "Senior" => 25000,
            "Junior" => 10000,
            _ => throw new UnsupportedSeniority($"{seniority} is not supported")
        };
}
