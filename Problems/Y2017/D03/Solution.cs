using Problems.Y2017.Common;
using Utilities.Cartesian;

namespace Problems.Y2017.D03;

/// <summary>
/// Spiral Memory: https://adventofcode.com/2017/day/3
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 3;
    
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
        return Vector2D.Distance(
            a: memory.LastPos,
            b: Vector2D.Zero,
            metric: DistanceMetric.Taxicab);
    }
    
    private static int GetFirstLargerValue(int threshold)
    {
        var memory = new Spiral();
        
        int ValueFunc(Spiral spiral) => spiral.NextPos.GetAdjacentSet(DistanceMetric.Chebyshev).Sum(pos => spiral[pos]);
        bool StopPredicate(Spiral spiral) => spiral.LastVal > threshold;

        memory.Build(ValueFunc, StopPredicate);
        return memory.LastVal;
    }
}