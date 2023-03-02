using Problems.Attributes;
using Problems.Y2022.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D09;

/// <summary>
/// Rope Bridge: https://adventofcode.com/2022/day/9
/// </summary>
[Favourite("Rope Bridge", Topics.Vectors, Difficulty.Medium)]
public class Solution : SolutionBase2022
{
    private static readonly Dictionary<string, Vector2D> VectorMap = new()
    {
        { "U", Vector2D.Up },
        { "D", Vector2D.Down },
        { "L", Vector2D.Left },
        { "R", Vector2D.Right },
    };

    public override int Day => 9;
    
    public override object Run(int part)
    {
        var movements = ParseHeadMovements(GetInputLines());
        return part switch
        {
            1 =>  CountDistinctKnotPositions(movements, 2),
            2 =>  CountDistinctKnotPositions(movements, 10),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountDistinctKnotPositions(Queue<Vector2D> movements, int numKnots)
    {
        var knots = new Vector2D[numKnots];
        var visited = new HashSet<Vector2D>();

        while (movements.Count > 0)
        {
            knots[0] += movements.Dequeue();
            
            for (var i = 1; i < numKnots; i++)
            {
                if (!knots[i].IsAdjacentTo(knots[i - 1], DistanceMetric.Chebyshev))
                {
                    knots[i] += Vector2D.Normalize(knots[i - 1] - knots[i]);
                }
            }
            
            visited.Add(knots[numKnots - 1]);
        }

        return visited.Count;
    }
    
    private static Queue<Vector2D> ParseHeadMovements(IEnumerable<string> lines)
    {
        var queue = new Queue<Vector2D>();
        foreach (var line in lines)
        {
            var cmd = line.Split(' ');
            var vector = VectorMap[cmd[0]];
            var numMotions = int.Parse(cmd[1]);

            for (var i = 0; i < numMotions; i++)
            {
                queue.Enqueue(vector);
            }
        }

        return queue;
    }
}