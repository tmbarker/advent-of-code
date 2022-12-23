using System.Text.RegularExpressions;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D22;

public static class MapData
{
    private const string InstructionRegex = @"(\d+|[LR])";
    private const string Left = "L";
    private const string Right = "R";
    private const char Free = '.';
    private const char Blocked = '#';
    
    public const int RowMultiplier = 1000;
    public const int ColMultiplier = 4;

    public static readonly Dictionary<Vector2D, int> FacingValues = new()
    {
        { Vector2D.Right, 0 },
        { Vector2D.Down, 1 },
        { Vector2D.Left, 2 },
        { Vector2D.Up, 3 },
    };

    // TODO: WIP - Don't want to have to hardcode this
    public static bool TryMoveBetweenFaces(Pose pose, Grid2D<Square> board)
    {
        var targetPos = Vector2D.PositiveInfinity;
        var targetFacing = Vector2D.Zero;
        
        var x = pose.Pos.X;
        var y = pose.Pos.Y;
        var face = pose.Facing;

        // 1 -> 2 // Done
        if (y == 11 && face == Vector2D.Up)
        {
            targetPos = new Vector2D(11 - x, 7);
            targetFacing = Vector2D.Down;
        }
        
        // 2 -> 1 // Done
        if (y == 7 && face == Vector2D.Up)
        {
            targetPos = new Vector2D(8 + (3 - x), 11);
            targetFacing = Vector2D.Down;
        }

        // 1 -> 3 // Done
        if (x == 8 && face == Vector2D.Left)
        {
            pose.Pos = new Vector2D(4 + (11 - y), 7);
            pose.Facing = Vector2D.Down;
        }
        
        // 3 -> 1 // Done
        if (y == 7 && face == Vector2D.Up)
        {
            pose.Pos = new Vector2D(8, 8 + (7 - x));
            pose.Facing = Vector2D.Right;
        }
        
        // 1 -> 6 // Done
        if (x == 11 && face == Vector2D.Right)
        {
            pose.Pos = new Vector2D(12 + (11 - y), 3);
            pose.Facing = Vector2D.Down;
        }
        
        // 6 -> 1 // Done
        if (y == 3 && face == Vector2D.Up)
        {
            pose.Pos = new Vector2D(8 + (x - 12), 3);
            pose.Facing = Vector2D.Left;
        }

        // 2 -> 6 // Done
        if (x == 0 && face == Vector2D.Left)
        {
            pose.Pos = new Vector2D(12 + (y - 4) ,0);
            pose.Facing = Vector2D.Up;
        }
        
        // 6 -> 2 // Done
        if (y == 0 && face == Vector2D.Down)
        {
            pose.Pos = new Vector2D(0, 4 + (12 - x));
            pose.Facing = Vector2D.Right;
        }
        
        // 4 -> 6 // Done
        if (x == 11 && face == Vector2D.Right)
        {
            pose.Pos = new Vector2D(12 + (y - 4), 3);
            pose.Facing = Vector2D.Down;
        }
        
        // 6 -> 4 // Done
        if (y == 3 && face == Vector2D.Up)
        {
            pose.Pos = new Vector2D(11, 4 + (x - 12));
            pose.Facing = Vector2D.Left;
        }
        
        // 3 -> 5 // Done
        if (y == 4 && face == Vector2D.Down)
        {
            pose.Pos = new Vector2D(8, x - 4);
            pose.Facing = Vector2D.Right;
        }
        
        // 5 -> 3 // Done
        if (x == 8 && face == Vector2D.Left)
        {
            pose.Pos = new Vector2D(4 + y, 4);
            pose.Facing = Vector2D.Up;
        }
        
        // 2 -> 5 // Done <-- suspect
        if (x == 4 && face == Vector2D.Down)
        {
            pose.Pos = new Vector2D(11 - x, 0);
            pose.Facing = Vector2D.Up;
        }
        
        // 5 -> 2 // Done <-- suspect
        if (y == 0 && face == Vector2D.Down)
        {
            pose.Pos = new Vector2D(11 - x, 4);
            pose.Facing = Vector2D.Up;
        }
        
        if (!board.IsInDomain(targetPos) || board[targetPos] != Square.Free)
        {
            return false;
        }

        pose.Pos = targetPos;
        pose.Facing = targetFacing;
        return true;
    }

    public static void Parse(IList<string> input, out Grid2D<Square> board, out IEnumerable<Instruction> instructions)
    {
        var freePosSet = new HashSet<Vector2D>();
        var blockedPosSet = new HashSet<Vector2D>();

        for (var i = 0; i < input.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                break;
            }

            ParsePositionsFromLine(input[i], i, freePosSet, blockedPosSet);
        }

        var cols = Math.Max(freePosSet.Max(p => p.X), blockedPosSet.Max(p => p.X)) + 1;
        var rows = Math.Max(freePosSet.Max(p => p.Y), blockedPosSet.Max(p => p.Y)) + 1;

        instructions = ParseInstructions(input.Last());
        board = Grid2D<Square>.WithDimensions(rows, cols);
        
        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var pos = new Vector2D(x, y);
            var square = freePosSet.Contains(pos) ? Square.Free :
                blockedPosSet.Contains(pos) ? Square.Blocked : Square.OutOfBounds;
            
            board[x, rows - y - 1] = square;
        }
    }
    
    private static void ParsePositionsFromLine(string line, int y, ISet<Vector2D> freePosSet, ISet<Vector2D> blockedPosSet)
    {
        for (var x = 0; x < line.Length; x++)
        {
            switch (line[x])
            {
                case Free:
                    freePosSet.Add(new Vector2D(x, y));
                    break;
                case Blocked:
                    blockedPosSet.Add(new Vector2D(x, y));
                    break;
            }
        }
    }

    private static IEnumerable<Instruction> ParseInstructions(string line)
    {
        var instructions = new List<Instruction>();
        var matches = Regex.Matches(line, InstructionRegex);

        foreach (Match match in matches)
        {
            switch (match.Value)
            {
                case Left:
                    instructions.Add(new Instruction(0, Rotation2D.Ccw90));
                    continue;
                case Right:
                    instructions.Add(new Instruction(0, Rotation2D.Cw90));
                    continue;
                default:
                    instructions.Add(new Instruction(int.Parse(match.Value), Rotation2D.Zero));
                    break;
            }
        }

        return instructions;
    }
}