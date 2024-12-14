using System.Text;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D02;

[PuzzleInfo("Bathroom Security", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<Vec2D, char> Square = new()
    {
        { new Vec2D(X: -1, Y:  1), '1' },
        { new Vec2D(X:  0, Y:  1), '2' },
        { new Vec2D(X:  1, Y:  1), '3' },
        { new Vec2D(X: -1, Y:  0), '4' },
        { new Vec2D(X:  0, Y:  0), '5' },
        { new Vec2D(X:  1, Y:  0), '6' },
        { new Vec2D(X: -1, Y: -1), '7' },
        { new Vec2D(X:  0, Y: -1), '8' },
        { new Vec2D(X:  1, Y: -1), '9' }
    };
    
    private static readonly Dictionary<Vec2D, char> Diamond = new()
    {
        { new Vec2D(X:  0, Y:  2), '1' },
        { new Vec2D(X: -1, Y:  1), '2' },
        { new Vec2D(X:  0, Y:  1), '3' },
        { new Vec2D(X:  1, Y:  1), '4' },
        { new Vec2D(X: -2, Y:  0), '5' },
        { new Vec2D(X: -1, Y:  0), '6' },
        { new Vec2D(X:  0, Y:  0), '7' },
        { new Vec2D(X:  1, Y:  0), '8' },
        { new Vec2D(X:  2, Y:  0), '9' },
        { new Vec2D(X: -1, Y: -1), 'A' },
        { new Vec2D(X:  0, Y: -1), 'B' },
        { new Vec2D(X:  1, Y: -1), 'C' },
        { new Vec2D(X:  0, Y: -2), 'D' }
    };
    
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => BuildCode(pos: new Vec2D(X:  0, Y: 0), map: Square),
            2 => BuildCode(pos: new Vec2D(X: -2, Y: 0), map: Diamond),
            _ => PuzzleNotSolvedString
        };
    }

    private string BuildCode(Vec2D pos, Dictionary<Vec2D, char> map)
    {
        var instructions = GetInputLines();
        var sb = new StringBuilder();

        foreach (var instruction in instructions)
        {
            foreach (var step in instruction)
            {
                var move = step switch
                {
                    'U' => Vec2D.Up,
                    'D' => Vec2D.Down,
                    'L' => Vec2D.Left,
                    'R' => Vec2D.Right,
                    _ => throw new NoSolutionException()
                };

                if (map.ContainsKey(pos + move))
                {
                    pos += move;
                }
            }

            sb.Append(map[pos]);
        }

        return sb.ToString();
    }
}