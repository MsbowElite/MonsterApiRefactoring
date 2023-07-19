namespace Lib.ProblemSolving;

public static class Challenge2
{
    public static int DiceFacesCalculator(int dice1, int dice2, int dice3)
    {
        List<int> list = new();

        list.Add(dice1);
        list.Add(dice2);    
        list.Add(dice3);

        foreach(int i in list)
        {
            list.Where(w => w == i).Count();

        }

        return list.Count;
    }
}