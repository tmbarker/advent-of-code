using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2022.D09;

/// <summary>
/// Rope Bridge: https://adventofcode.com/2022/day/9
/// </summary>
public class Solution : SolutionBase2022
{
    private const char Delimiter = ' ';
    private const int NumKnotsPart1 = 2;
    private const int NumKnotsPart2 = 10;
    
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
        var movements = ParseHeadMovements(GetInput());
        
        return part switch
        {
            0 => CountDistinctKnotPositions(movements, NumKnotsPart1),
            1 => CountDistinctKnotPositions(movements, NumKnotsPart2),
            _ => ProblemNotSolvedString,
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
                if (!knots[i].IsAdjacentTo(knots[i - 1]))
                {
                    knots[i] += Vector2D.Normalize(knots[i - 1] - knots[i]);
                }
            }
            
            visited.EnsureContains(knots[numKnots - 1]);
        }

        return visited.Count;
    }
    
    private static Queue<Vector2D> ParseHeadMovements(IEnumerable<string> lines)
    {
        var queue = new Queue<Vector2D>();
        foreach (var line in lines)
        {
            var cmd = line.Split(Delimiter);
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