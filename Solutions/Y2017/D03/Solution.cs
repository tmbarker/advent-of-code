using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D03;

/// <summary>
/// Spiral Memory: https://adventofcode.com/2017/day/3
/// </summary>
[PuzzleInfo("Spiral Memory", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var value = int.Parse(input);
        
        return part switch
        {
            1 => GetDistanceToOrigin(value),
            2 => GetFirstLargerValue(value),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetDistanceToOrigin(int square)
    {
        var memory = new Spiral();
        memory.Build(
            valueFunc: spiral => spiral.LastVal + 1,
            stopFunc: spiral => spiral.LastVal == square);
        
        return memory.LastPos.Magnitude(Metric.Taxicab);
    }
    
    private static int GetFirstLargerValue(int threshold)
    {
        var memory = new Spiral();
        memory.Build(
            valueFunc: spiral => spiral.NextPos.GetAdjacentSet(Metric.Chebyshev).Sum(pos => spiral[pos]),
            stopFunc: spiral => spiral.LastVal > threshold);
        
        return memory.LastVal;
    }
}