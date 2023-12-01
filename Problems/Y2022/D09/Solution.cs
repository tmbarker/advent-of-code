using Problems.Attributes;
using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D09;

/// <summary>
/// Rope Bridge: https://adventofcode.com/2022/day/9
/// </summary>
[Favourite("Rope Bridge", Topics.Vectors, Difficulty.Medium)]
public class Solution : SolutionBase
{
    private static readonly Dictionary<string, Vector2D> VectorMap = new()
    {
        { "U", Vector2D.Up },
        { "D", Vector2D.Down },
        { "L", Vector2D.Left },
        { "R", Vector2D.Right }
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var steps = ParseHeadMovements(input);
        
        return part switch
        {
            1 => CountKnotPositions(steps, count: 2),
            2 => CountKnotPositions(steps, count: 10),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountKnotPositions(IEnumerable<Vector2D> steps, int count)
    {
        var knots = new Vector2D[count];
        var visited = new HashSet<Vector2D> { Vector2D.Zero };

        foreach (var step in steps)
        {
            knots[0] += step;
            for (var i = 1; i < count; i++)
            {
                if (!knots[i].IsAdjacentTo(knots[i - 1], Metric.Chebyshev))
                {
                    knots[i] += Vector2D.Normalize(knots[i - 1] - knots[i]);
                }
            }
            
            visited.Add(knots[count - 1]);
        }

        return visited.Count;
    }
    
    private static IEnumerable<Vector2D> ParseHeadMovements(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var tokens = line.Split(' ');
            var vector = VectorMap[tokens[0]];
            var numMotions = int.Parse(tokens[1]);

            for (var i = 0; i < numMotions; i++)
            {
                yield return vector;
            }
        }
    }
}