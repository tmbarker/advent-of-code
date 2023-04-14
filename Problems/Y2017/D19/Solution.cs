using System.Text;
using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2017.D19;

/// <summary>
/// A Series of Tubes: https://adventofcode.com/2017/day/19
/// </summary>
public class Solution : SolutionBase
{
    private const char Junction = '+';
    private static readonly HashSet<char> Traversable = new() { '|', '-', '+' };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var map = Grid2D<char>.MapChars(input, c => c);
        var start = new Vector2D(
            x: input[0].IndexOf('|'),
            y: map.Height - 1);

        return part switch
        {
            1 => Traverse(map, start).Letters,
            2 => Traverse(map, start).Steps,
            _ => ProblemNotSolvedString
        };
    }

    private static PathSummary Traverse(Grid2D<char> map, Vector2D start)
    {
        var letters = new StringBuilder();
        var steps = 1;
        
        var face = Vector2D.Down;
        var pos = start;

        while (CanMoveTo(map, pos: pos + face))
        {
            pos += face;
            steps++;
            
            if (char.IsLetter(map[pos]))
            {
                letters.Append(map[pos]);
            }

            if (map[pos] == Junction)
            {
                face = CanMoveTo(map, pos + (Vector2D)(Rotation3D.Positive90Z * face))
                    ? Rotation3D.Positive90Z * face
                    : Rotation3D.Negative90Z * face;
            }
        }

        return new PathSummary(
            Letters: letters.ToString(),
            Steps: steps);
    }

    private static bool CanMoveTo(Grid2D<char> map, Vector2D pos)
    {
        return map.IsInDomain(pos) && (Traversable.Contains(map[pos]) || char.IsLetter(map[pos]));
    }

    private readonly record struct PathSummary(string Letters, int Steps);
}