using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2023.D06;

/// <summary>
/// Wait For It: https://adventofcode.com/2023/day/6
/// </summary>
public sealed class Solution : SolutionBase
{
    private readonly record struct Race(long Time, long Distance);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => AggregateStrategies(trimInput: false),
            2 => AggregateStrategies(trimInput: true),
            _ => ProblemNotSolvedString
        };
    }
    
    private long AggregateStrategies(bool trimInput)
    {
        var input = GetInputLines();
        if (trimInput)
        {
            input[0] = input[0].RemoveWhitespace();
            input[1] = input[1].RemoveWhitespace();
        }
        
        var times = input[0].ParseLongs();
        var distances = input[1].ParseLongs();
        
        return times
            .Zip(distances, (time, distance) => new Race(time, distance))
            .Aggregate(
                seed: 1L, 
                func: (product, race) => product * CountStrategies(race));
    }
    
    private static long CountStrategies(Race race)
    {
        var zeroes = SolveQuadratic(a: -1, b: race.Time, c: -1 * race.Distance);
        var min = Math.BitIncrement(zeroes.Min());
        var max = Math.BitDecrement(zeroes.Max());

        return (long)(Math.Floor(max) - Math.Ceiling(min) + 1);
    }
    
    private static double[] SolveQuadratic(long a, long b, long c)
    {
        var zeroes = new double[2];
        var discriminant = b * b - 4.0 * a * c;

        zeroes[0] = (-b + Math.Sqrt(discriminant)) / (2.0 * a);
        zeroes[1] = (-b - Math.Sqrt(discriminant)) / (2.0 * a);
        
        return zeroes;
    }
}