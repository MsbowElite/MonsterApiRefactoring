namespace Lib.ProblemSolving;

public static class Challenge2
{
    public static int DiceFacesCalculator(int dice1, int dice2, int dice3)
    {
        List<int> list = new()
        {
            dice1,
            dice2,
            dice3
        };

        if (list.Any(a => a > 6) || list.Any(a => a <= 0))
            throw new Exception("Dice out of number range");

        var groupByList = from x in list
                          group x by x into g
                          let count = g.Count()
                          orderby count descending
                          select new { Value = g.Key, Count = count };

        var moreThenOne = groupByList.FirstOrDefault(w => w.Count > 1);
        if (moreThenOne is not null)
            return moreThenOne.Value * moreThenOne.Count;
        else
            return list.Max();
    }
}