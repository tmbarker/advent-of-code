using Problems.Y2021.Common;

namespace Problems.Y2021.D07;

/// <summary>
/// The Treachery of Whales: https://adventofcode.com/2021/day/7
/// </summary>
public class Solution : SolutionBase2021
{
    private const char Delimiter = ',';
    
    public override int Day => 7;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => CalculateMinCumulativeCost(n => n),
            1 => CalculateMinCumulativeCost(n => n * (n + 1)/2),
            _ => ProblemNotSolvedString,
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
            .Split(Delimiter)
            .Select(int.Parse)
            .ToList();
    }
}