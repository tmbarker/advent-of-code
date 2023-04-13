using Problems.Common;
using Problems.Y2022.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2022.D24;

/// <summary>
/// Blizzard Basin: https://adventofcode.com/2022/day/24
/// </summary>
public class Solution : SolutionBase2022
{
    private const char Empty = '.';
    private const char Wall = '#';
    
    private static readonly Dictionary<char, Vector2D> VectorMap = new()
    {
        {'^', Vector2D.Up},
        {'v', Vector2D.Down},
        {'<', Vector2D.Left},
        {'>', Vector2D.Right},
    };

    public override int Day => 24;
    
    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var field, out var start, out var end, out var blizzards);
        
        return part switch
        {
            1 => Traverse(field, start, end, blizzards),
            2 => Navigate(field, start, end, blizzards),
            _ => ProblemNotSolvedString
        };
    }

    private static int Navigate(Grid2D<char> field, Vector2D start, Vector2D end, IList<Blizzard> blizzards)
    {
        var sum = 0;
        sum += Traverse(field, start, end, blizzards);
        sum += Traverse(field, end, start, blizzards);
        sum += Traverse(field, start, end, blizzards);
        
        return sum;
    }
    
    private static int Traverse(Grid2D<char> field, Vector2D start, Vector2D end, IList<Blizzard> blizzards)
    {
        var t = 1;
        var activePaths = new HashSet<Vector2D> { start };

        while (activePaths.Count > 0)
        {
            AdvanceBlizzards(field, blizzards);
            foreach (var pathHead in activePaths.Freeze())
            {
                var canWait = blizzards.All(b => b.Pos != pathHead);
                var moves = GetLegalMoves(pathHead, start, field, blizzards);

                if (!canWait)
                {
                    activePaths.Remove(pathHead);
                }

                foreach (var move in moves)
                {
                    if (move == end)
                    {
                        return t;
                    }
                    activePaths.Add(move);
                }
            }
            
            t++;
        }

        throw new NoSolutionException();
    }

    private static void AdvanceBlizzards(Grid2D<char> field, IEnumerable<Blizzard> blizzards)
    {
        foreach (var blizzard in blizzards)
        {
            if (field[blizzard.Course] == Empty)
            {
                blizzard.Pos = blizzard.Course;
                continue;
            }

            var respawn = blizzard.Pos - blizzard.Heading;
            while (field[respawn] != Wall)
            {
                respawn -= blizzard.Heading;
            }

            blizzard.Pos = respawn + blizzard.Heading;
        }
    }

    private static List<Vector2D> GetLegalMoves(Vector2D pathHead, Vector2D start, Grid2D<char> field, IEnumerable<Blizzard> blizzards)
    {
        return pathHead
            .GetAdjacentSet(Metric.Taxicab)
            .Where(move => IsMoveAllowed(move, start, field, blizzards))
            .ToList();
    }
    
    private static bool IsMoveAllowed(Vector2D move, Vector2D start, Grid2D<char> field, IEnumerable<Blizzard> blizzards)
    {
        return
            move != start &&
            field.IsInDomain(move) &&
            field[move] == Empty &&
            blizzards.All(b => b.Pos != move);
    }
    
    private static void ParseInput(IList<string> input, out Grid2D<char> field, out Vector2D start, out Vector2D end, out List<Blizzard> blizzards)
    {
        var rows = input.Count;
        var cols = input[0].Length;

        blizzards = new List<Blizzard>();
        field = Grid2D<char>.WithDimensions(rows, cols);
        start = new Vector2D(input[0].IndexOf(Empty), rows - 1);
        end = new Vector2D(input[rows - 1].IndexOf(Empty), 0);

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var pos = new Vector2D(x, rows - y - 1);
            var value = input[y][x];

            field[pos] = value == Empty || VectorMap.ContainsKey(value) 
                ? Empty 
                : Wall;
            
            if (VectorMap.TryGetValue(value, out var heading))
            {
                blizzards.Add(new Blizzard(pos, heading));
            }
        }
    }
}