using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D22;

public static class MapData
{
    public const int RowFactor = 1000;
    public const int ColFactor = 4;

    public static readonly Dictionary<Vec2D, int> FacingOffset = new()
    {
        { Vec2D.Right, 0 },
        { Vec2D.Down,  1 },
        { Vec2D.Left,  2 },
        { Vec2D.Up,    3 }
    };
    
    public static bool TryMoveBetweenFaces(Pose2D pose, Grid2D<Square> board, out Pose2D result)
    {
        var targetPos = Vec2D.PositiveInfinity;
        var targetFace = Vec2D.Zero;
        
        var x = pose.Pos.X;
        var y = pose.Pos.Y;
        var face = pose.Face;
        
        //  The input is structured like a cube net, the below conditionals manually map face edges to
        //  the constructed adjacent face edges:
        //
        //     1 2
        //     3  
        //   4 5
        //   6
        
        // 1 -> 6
        if (y == 199 && x is >= 50 and < 100 && face == Vec2D.Up)
        {
            targetPos = new Vec2D(0, 99 - x);
            targetFace = Vec2D.Right;
        }
        
        // 6 -> 1
        if (x == 0 && y is >= 0 and < 50 && face == Vec2D.Left)
        {
            targetPos = new Vec2D(99 - y, 199);
            targetFace = Vec2D.Down;
        }
        
        // 2 -> 6
        if (y == 199 && x is >= 100 and < 150 && face == Vec2D.Up)
        {
            targetPos = new Vec2D(x - 100, 0);
            targetFace = Vec2D.Up;
        }
        
        // 6 -> 2
        if (y == 0 && x is >= 0 and < 50 && face == Vec2D.Down)
        {
            targetPos = new Vec2D(100 + x, 199);
            targetFace = Vec2D.Down;
        }
        
        // 1 -> 4
        if (x == 50 && y is >= 150 and < 200 && face == Vec2D.Left)
        {
            targetPos = new Vec2D(0, 249 - y);
            targetFace = Vec2D.Right;
        }
        
        // 4 -> 1
        if (x == 0 && y is >= 50 and < 100 && face == Vec2D.Left)
        {
            targetPos = new Vec2D(50, 249 - y);
            targetFace = Vec2D.Right;
        }

        // 2 -> 5
        if (x == 149 && y is >= 150 and < 200 && face == Vec2D.Right)
        {
            targetPos = new Vec2D(99, 249 - y);
            targetFace = Vec2D.Left;
        }
        
        // 5 -> 2
        if (x == 99 && y is >= 50 and < 100 && face == Vec2D.Right)
        {
            targetPos = new Vec2D(149, 249 - y);
            targetFace = Vec2D.Left;
        }
        
        // 4 -> 3
        if (y == 99 && x is >= 0 and < 50 && face == Vec2D.Up)
        {
            targetPos = new Vec2D(50, 149 - x);
            targetFace = Vec2D.Right;
        }
        
        // 3 -> 4
        if (x == 50 && y is >= 100 and < 150 && face == Vec2D.Left)
        {
            targetPos = new Vec2D(149 - y, 99);
            targetFace = Vec2D.Down;
        }
        
        // 3 -> 2
        if (x == 99 && y is >= 100 and < 150 && face == Vec2D.Right)
        {
            targetPos = new Vec2D(249 - y, 150);
            targetFace = Vec2D.Up;
        }
        
        // 2 -> 3
        if (y == 150 && x is >= 100 and < 150 && face == Vec2D.Down)
        {
            targetPos = new Vec2D(99, 249 - x);
            targetFace = Vec2D.Left;
        }
        
        // 6 -> 5
        if (x == 49 && y is >= 0 and < 50 && face == Vec2D.Right)
        {
            targetPos = new Vec2D(99 - y, 50);
            targetFace = Vec2D.Up;
        }
        
        // 5 -> 6
        if (y == 50 && x is >= 50 and < 100 && face == Vec2D.Down)
        {
            targetPos = new Vec2D(49, 99 - x);
            targetFace = Vec2D.Left;
        }

        if (!board.Contains(targetPos) || board[targetPos] != Square.Free)
        {
            result = default;
            return false;
        }
        
        result = new Pose2D(targetPos, targetFace);
        return true;
    }

    public static void Parse(string[] input, out Grid2D<Square> board, out IEnumerable<Instruction> instructions)
    {
        var freePosSet = new HashSet<Vec2D>();
        var blockedPosSet = new HashSet<Vec2D>();

        for (var i = 0; i < input.Length - 2; i++)
        {
            ParsePositionsFromLine(input[i], i, freePosSet, blockedPosSet);
        }

        var cols = Math.Max(freePosSet.Max(p => p.X), blockedPosSet.Max(p => p.X)) + 1;
        var rows = Math.Max(freePosSet.Max(p => p.Y), blockedPosSet.Max(p => p.Y)) + 1;

        instructions = ParseInstructions(input[^1]);
        board = Grid2D<Square>.WithDimensions(rows, cols);
        
        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var pos = new Vec2D(x, y);
            var square = freePosSet.Contains(pos) ? Square.Free :
                blockedPosSet.Contains(pos) ? Square.Blocked : Square.OutOfBounds;
            
            board[x, rows - y - 1] = square;
        }
    }

    private static void ParsePositionsFromLine(string line, int y, 
        HashSet<Vec2D> freePosSet, HashSet<Vec2D> blockedPosSet)
    {
        for (var x = 0; x < line.Length; x++)
        {
            switch (line[x])
            {
                case '.':
                    freePosSet.Add(new Vec2D(x, y));
                    break;
                case '#':
                    blockedPosSet.Add(new Vec2D(x, y));
                    break;
            }
        }
    }

    private static IEnumerable<Instruction> ParseInstructions(string line)
    {
        foreach (Match match in Regex.Matches(line, @"(\d+|[LR])"))
        {
            yield return match.Value switch
            {
                "L" => new Instruction(steps: 0, Rot3D.P90Z),
                "R" => new Instruction(steps: 0, Rot3D.N90Z),
                _   => new Instruction(steps: match.ParseInt(), Rot3D.Zero)
            };

        }
    }
}