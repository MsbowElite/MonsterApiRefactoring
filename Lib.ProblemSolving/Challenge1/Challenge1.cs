namespace Lib.ProblemSolving;

public static class Challenge1
{
    public static Challenge1Result FractionsCalculator(int[] numbers)
    {
        Challenge1Result challenge1Result = new();
        var numbersList = numbers.ToList();
        challenge1Result.Positives = GetPositiveFraction(numbersList);
        challenge1Result.Negatives = GetNegativeFraction(numbersList);
        challenge1Result.Zeros = GetZeroFraction(numbersList);

        return challenge1Result;
    }

    private static decimal GetPositiveFraction(List<int> numbersList)
    {
        return numbersList.Where(w => w > 0).Count() / (decimal)numbersList.Count();
    }

    private static decimal GetNegativeFraction(List<int> numbersList)
    {
        return numbersList.Where(w => w < 0).Count() / (decimal)numbersList.Count();
    }

    private static decimal GetZeroFraction(List<int> numbersList)
    {
        return numbersList.Where(w => w == 0).Count() / (decimal)numbersList.Count();
    }
}

public class Challenge1Result
{
    public decimal Positives { get; set; }
    public decimal Negatives { get; set; }
    public decimal Zeros { get; set; }
}