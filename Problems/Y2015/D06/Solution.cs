using Problems.Common;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2015.D06;

/// <summary>
/// Probably a Fire Hazard: https://adventofcode.com/2015/day/6
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var instructions = input.Select(ParseInstruction);
        
        return part switch
        {
            1 => ExecuteBoolean(instructions),
            2 => ExecuteIntegral(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static long ExecuteBoolean(IEnumerable<Instruction> instructions)
    {
        var map = new DefaultDictionary<Vector2D, bool>(defaultValue: false);
        
        foreach (var (aabb, action) in instructions)
        foreach (var pos in aabb)
        {
            map[pos] = action switch
            {
                Action.On => true,
                Action.Off => false,
                Action.Toggle => !map[pos],
                _ => throw new NoSolutionException()
            };
        }
        
        return map.Count(kvp => kvp.Value);
    }
    
    private static long ExecuteIntegral(IEnumerable<Instruction> instructions)
    {
        var map = new DefaultDictionary<Vector2D, int>(defaultValue: 0);
        
        foreach (var (aabb, action) in instructions)
        foreach (var pos in aabb)
        {
            switch (action)
            {
                case Action.On:
                    map[pos]++;
                    break;
                case Action.Off:
                    map[pos] = Math.Max(0, map[pos] - 1);
                    break;
                case Action.Toggle:
                    map[pos] += 2;
                    break;
                default:
                    throw new NoSolutionException();
            }
        }

        return map.Sum(kvp => kvp.Value);
    }
    
    private static Instruction ParseInstruction(string line)
    {
        var numbers = line.ParseInts();
        var min = new Vector2D(x: numbers[0], y: numbers[1]);
        var max = new Vector2D(x: numbers[2], y: numbers[3]);

        var action = line switch
        {
            not null when line.Contains("on")     => Action.On,
            not null when line.Contains("off")    => Action.Off,
            not null when line.Contains("toggle") => Action.Toggle,
            _ => throw new NoSolutionException()
        };

        return new Instruction(
            Aabb: new Aabb2D(extents: new[] { min, max }),
            Action: action);
    }

    private readonly record struct Instruction(Aabb2D Aabb, Action Action);

    private enum Action
    {
        On,
        Off,
        Toggle
    }
}