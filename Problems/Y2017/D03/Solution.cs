using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2017.D03;

/// <summary>
/// Spiral Memory: https://adventofcode.com/2017/day/3
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var value = int.Parse(input);
        
        return part switch
        {
            1 => GetDistanceToOrigin(value),
            2 => GetFirstLargerValue(value),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetDistanceToOrigin(int square)
    {
        var memory = new Spiral();
        
        int ValueFunc(Spiral spiral) => spiral.LastVal + 1;
        bool StopPredicate(Spiral spiral) => spiral.LastVal == square;

        memory.Build(ValueFunc, StopPredicate);
        return memory.LastPos.Magnitude(Metric.Taxicab);
    }
    
    private static int GetFirstLargerValue(int threshold)
    {
        var memory = new Spiral();
        
        int ValueFunc(Spiral spiral) => spiral.NextPos.GetAdjacentSet(Metric.Chebyshev).Sum(pos => spiral[pos]);
        bool StopPredicate(Spiral spiral) => spiral.LastVal > threshold;

        memory.Build(ValueFunc, StopPredicate);
        return memory.LastVal;
    }
}