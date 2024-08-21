using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D09;

[PuzzleInfo("Rope Bridge", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<string, Vec2D> VectorMap = new()
    {
        { "U", Vec2D.Up },
        { "D", Vec2D.Down },
        { "L", Vec2D.Left },
        { "R", Vec2D.Right }
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var steps = ParseHeadMovements(input);
        
        return part switch
        {
            1 => CountKnotPositions(steps, count: 2),
            2 => CountKnotPositions(steps, count: 10),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountKnotPositions(IEnumerable<Vec2D> steps, int count)
    {
        var knots = new Vec2D[count];
        var visited = new HashSet<Vec2D>(collection: [Vec2D.Zero]);

        foreach (var step in steps)
        {
            knots[0] += step;
            for (var i = 1; i < count; i++)
            {
                if (!Vec2D.IsAdjacent(a: knots[i], b: knots[i - 1], Metric.Chebyshev))
                {
                    knots[i] += Vec2D.Normalize(knots[i - 1] - knots[i]);
                }
            }
            
            visited.Add(knots[count - 1]);
        }

        return visited.Count;
    }
    
    private static IEnumerable<Vec2D> ParseHeadMovements(IEnumerable<string> lines)
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