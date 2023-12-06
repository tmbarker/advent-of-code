using Problems.Common;

namespace Problems.Y2021.D07;

/// <summary>
/// The Treachery of Whales: https://adventofcode.com/2021/day/7
/// </summary>
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
        
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var distinctPosition in distinctPositions)
        {
            var sum = positions.Sum(p => distanceCostFunc(Math.Abs(distinctPosition - p)));
            best = Math.Min(best, sum);
        }

        return best;
    }

    private IList<int> GetPositions()
    {
        return GetInputText()
            .Split(separator: ',')
            .Select(int.Parse)
            .ToList();
    }
}