using Problems.Attributes;
using Problems.Y2017.Common;
using Utilities.Cartesian;

namespace Problems.Y2017.D03;

/// <summary>
/// Spiral Memory: https://adventofcode.com/2017/day/3
/// </summary>
[Favourite("Spiral Memory", Topics.Math|Topics.Vectors, Difficulty.Medium)]
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
        var memory = new SpiralMemory();
        
        int ValueFunc(SpiralMemory mem) => mem.LastVal + 1;
        bool StopPredicate(SpiralMemory mem) => mem.LastVal == square;

        memory.Run(ValueFunc, StopPredicate);
        return Vector2D.Distance(
            a: memory.LastPos,
            b: Vector2D.Zero,
            metric: DistanceMetric.Taxicab);
    }
    
    private static int GetFirstLargerValue(int threshold)
    {
        var memory = new SpiralMemory();
        
        int ValueFunc(SpiralMemory mem) => mem.NextPos.GetAdjacentSet(DistanceMetric.Chebyshev).Sum(pos => mem[pos]);
        bool StopPredicate(SpiralMemory mem) => mem.LastVal > threshold;

        memory.Run(ValueFunc, StopPredicate);
        return memory.LastVal;
    }
}