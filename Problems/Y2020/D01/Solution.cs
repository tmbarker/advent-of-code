namespace Problems.Y2020.D01;

[PuzzleInfo("Report Repair", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var numbers = ParseInputLines(parseFunc: int.Parse).ToHashSet();
        return part switch
        {
            1 => GetSumPairProduct(targetSum: 2020, numbers),
            2 => GetSumTripletProduct(targetSum: 2020, numbers),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetSumPairProduct(int targetSum, IReadOnlySet<int> numbers)
    {
        foreach (var n1 in numbers)
        {
            var n2 = targetSum - n1;
            if (numbers.Contains(n2))
            {
                return n1 * n2;
            }
        }
        
        throw new NoSolutionException();
    }
    
    private static int GetSumTripletProduct(int targetSum, IReadOnlySet<int> numbers)
    {
        foreach (var n1 in numbers)
        foreach (var n2 in numbers)
        {
            var n3 = targetSum - n1 - n2;
            if (numbers.Contains(n3))
            {
                return n1 * n2 * n3;
            }
        }

        throw new NoSolutionException();
    }
}