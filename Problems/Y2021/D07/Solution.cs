using Utilities.Extensions;

namespace Problems.Y2021.D07;

[PuzzleInfo("The Treachery of Whales", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CalculateMinCumulativeCost(n => n),
            2 => CalculateMinCumulativeCost(n => n * (n + 1)/2),
            _ => ProblemNotSolvedString
        };
    }

    private int CalculateMinCumulativeCost(Func<int, int> distanceCostFunc)
    {
        var positions = GetPositions();
        var distinctPositions = positions.Distinct();
        var best = int.MaxValue;
        
        foreach (var distinct in distinctPositions)
        {
            best = Math.Min(
                val1: best,
                val2: positions.Sum(pos => distanceCostFunc(Math.Abs(distinct - pos))));
        }

        return best;
    }

    private int[] GetPositions()
    {
        return GetInputText().ParseInts();
    }
}