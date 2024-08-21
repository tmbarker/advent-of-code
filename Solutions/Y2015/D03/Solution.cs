using Utilities.Geometry.Euclidean;

namespace Solutions.Y2015.D03;

[PuzzleInfo("Perfectly Spherical Houses in a Vacuum", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vec2D> Map = new()
    {
        { '^', Vec2D.Up },
        { 'v', Vec2D.Down },
        { '<', Vec2D.Left },
        { '>', Vec2D.Right }
    };

    public override object Run(int part)
    {
        var steps = GetInputText();
        return part switch
        {
            1 => CountDistinctAlone(steps),
            2 => CountDistinctPair(steps),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountDistinctAlone(string steps)
    {
        var pos = Vec2D.Zero;
        var set = new HashSet<Vec2D>(collection: [Vec2D.Zero]);

        foreach (var step in steps)
        {
            pos += Map[step];
            set.Add(pos);
        }

        return set.Count;
    }
    
    private static int CountDistinctPair(string steps)
    {
        var set = new HashSet<Vec2D>(collection: [Vec2D.Zero]);
        var agents = new Dictionary<int, Vec2D>
        {
            { 0, Vec2D.Zero },
            { 1, Vec2D.Zero }
        };

        for (var i = 0; i < steps.Length; i++)
        {
            agents[i % 2] += Map[steps[i]];
            set.Add(agents[i % 2]);
        }

        return set.Count;
    }
}