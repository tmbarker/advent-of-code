using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2015.D03;

/// <summary>
/// Perfectly Spherical Houses in a Vacuum: https://adventofcode.com/2015/day/3
/// </summary>
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vector2D> Map = new()
    {
        { '^', Vector2D.Up },
        { 'v', Vector2D.Down },
        { '<', Vector2D.Left },
        { '>', Vector2D.Right }
    };

    public override object Run(int part)
    {
        var steps = GetInputText();
        return part switch
        {
            1 => CountDistinctAlone(steps),
            2 => CountDistinctPair(steps),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountDistinctAlone(string steps)
    {
        var pos = Vector2D.Zero;
        var set = new HashSet<Vector2D> { Vector2D.Zero };

        foreach (var step in steps)
        {
            pos += Map[step];
            set.Add(pos);
        }

        return set.Count;
    }
    
    private static int CountDistinctPair(string steps)
    {
        var set = new HashSet<Vector2D> { Vector2D.Zero };
        var agents = new Dictionary<int, Vector2D>
        {
            { 0, Vector2D.Zero },
            { 1, Vector2D.Zero }
        };

        for (var i = 0; i < steps.Length; i++)
        {
            agents[i % 2] += Map[steps[i]];
            set.Add(agents[i % 2]);
        }

        return set.Count;
    }
}