using Problems.Common;
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

    // TODO: Refactor - Currently all cube face adjacencies/mappings are handled by hardcoded conditionals
    public static bool TryMoveBetweenFaces(Pose pose, Grid2D<Square> board)
    {
        var targetPos = Vector2D.PositiveInfinity;
        var targetFacing = Vector2D.Zero;
        
        var x = pose.Pos.X;
        var y = pose.Pos.Y;
        var face = pose.Facing;

        // 1 -> 6
        if (y == 199 && x is >= 50 and < 100 && face == Vector2D.Up)
        {
            targetPos = new Vector2D(0, 99 - x);
            targetFacing = Vector2D.Right;
        }
        
        // 6 -> 1
        if (x == 0 && y is >= 0 and < 50 && face == Vector2D.Left)
        {
            targetPos = new Vector2D(99 - y, 199);
            targetFacing = Vector2D.Down;
        }
        
        // 2 -> 6
        if (y == 199 && x is >= 100 and < 150 && face == Vector2D.Up)
        {
            targetPos = new Vector2D(x - 100, 0);
            targetFacing = Vector2D.Up;
        }
        
        // 6 -> 2
        if (y == 0 && x is >= 0 and < 50 && face == Vector2D.Down)
        {
            targetPos = new Vector2D(100 + x, 199);
            targetFacing = Vector2D.Down;
        }
        
        // 1 -> 4
        if (x == 50 && y is >= 150 and < 200 && face == Vector2D.Left)
        {
            targetPos = new Vector2D(0, 249 - y);
            targetFacing = Vector2D.Right;
        }
        
        // 4 -> 1
        if (x == 0 && y is >= 50 and < 100 && face == Vector2D.Left)
        {
            targetPos = new Vector2D(50, 249 - y);
            targetFacing = Vector2D.Right;
        }

        // 2 -> 5
        if (x == 149 && y is >= 150 and < 200 && face == Vector2D.Right)
        {
            targetPos = new Vector2D(99, 249 - y);
            targetFacing = Vector2D.Left;
        }
        
        // 5 -> 2
        if (x == 99 && y is >= 50 and < 100 && face == Vector2D.Right)
        {
            targetPos = new Vector2D(149, 249 - y);
            targetFacing = Vector2D.Left;
        }
        
        // 4 -> 3
        if (y == 99 && x is >= 0 and < 50 && face == Vector2D.Up)
        {
            targetPos = new Vector2D(50, 149 - x);
            targetFacing = Vector2D.Right;
        }
        
        // 3 -> 4
        if (x == 50 && y is >= 100 and < 150 && face == Vector2D.Left)
        {
            targetPos = new Vector2D(149 - y, 99);
            targetFacing = Vector2D.Down;
        }
        
        // 3 -> 2
        if (x == 99 && y is >= 100 and < 150 && face == Vector2D.Right)
        {
            targetPos = new Vector2D(249 - y, 150);
            targetFacing = Vector2D.Up;
        }
        
        // 2 -> 3
        if (y == 150 && x is >= 100 and < 150 && face == Vector2D.Down)
        {
            targetPos = new Vector2D(99, 249 - x);
            targetFacing = Vector2D.Left;
        }
        
        // 6 -> 5
        if (x == 49 && y is >= 0 and < 50 && face == Vector2D.Right)
        {
            targetPos = new Vector2D(99 - y, 50);
            targetFacing = Vector2D.Up;
        }
        
        // 5 -> 6
        if (y == 50 && x is >= 50 and < 100 && face == Vector2D.Down)
        {
            targetPos = new Vector2D(49, 99 - x);
            targetFacing = Vector2D.Left;
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
                    instructions.Add(new Instruction(0, Rotation3D.Positive90Z));
                    continue;
                case Right:
                    instructions.Add(new Instruction(0, Rotation3D.Negative90Z));
                    continue;
                default:
                    instructions.Add(new Instruction(int.Parse(match.Value), Rotation3D.Zero));
                    break;
            }
        }

        return instructions;
    }

    // TODO: Remove after hardcoded cube mapping logic is refactored
    #region TEST MEMBERS

    private const int FaceSize = 50;
    private static readonly HashSet<Vector2D> NetFaceCoords = new()
    {
        FaceSize * new Vector2D(0, 0),
        FaceSize * new Vector2D(0, 1),
        FaceSize * new Vector2D(1, 1),
        FaceSize * new Vector2D(1, 2),
        FaceSize * new Vector2D(1, 3),
        FaceSize * new Vector2D(2, 3),
    };
    
    public static void TestCubeFaceMappings(Grid2D<Square> board)
    {
        foreach (var facePos in NetFaceCoords)
        {
            TestCubeFaceMapping(facePos, board);
        }
    }

    private static void TestCubeFaceMapping(Vector2D faceOrigin, Grid2D<Square> board)
    {
        var faceVertices = new List<Vector2D>
        {
            faceOrigin + (FaceSize - 1) * Vector2D.Zero,
            faceOrigin + (FaceSize - 1) * Vector2D.Up,
            faceOrigin + (FaceSize - 1) * Vector2D.One,
            faceOrigin + (FaceSize - 1) * Vector2D.Right,
        };

        for (var i = 0; i < faceVertices.Count - 1; i++)
        {
            var fromVertex = faceVertices[i];
            var toVertex = faceVertices[i + 1];
            var step = Vector2D.Normalize(toVertex - fromVertex);
            var fromFacing = (Vector2D)(Rotation3D.Positive90Z * step);
            var current = fromVertex;

            // Don't need to traverse edges which aren't on the perimeter of the net
            var isInteriorEdge = board.IsInDomain(current + fromFacing) && board[current + fromFacing] != Square.OutOfBounds;
            if (isInteriorEdge)
            {
                continue;
            }
             
            // Iterate along the edge
            while (current != toVertex + step)
            {
                // We don't need to verify cases where the edge position is unreachable due to being blocked, or the
                // mapped to position is blocked, there is no symmetry to verify in these cases
                var fromPose = new Pose(current, fromFacing);
                if (board[fromPose.Pos] == Square.Blocked || !TryMoveBetweenFaces(fromPose, board))
                {
                    current += step;
                    continue;
                }

                // If we can map to a position, we must be able to map back to it
                var returnFacing = -1 * fromPose.Facing;
                var returnPose = new Pose(fromPose.Pos, returnFacing);
                if (!TryMoveBetweenFaces(returnPose, board))
                {
                    Console.WriteLine($"Cannot return to {current} from {returnPose.Pos}");
                    throw new NoSolutionException();
                }

                // If we map to a position, we must map back to the same from position
                if (returnPose.Pos != current)
                {
                    Console.WriteLine($"Asymmetry detected: From => {current}");
                    throw new NoSolutionException();
                }
                
                current += step;
            }
        }
    }

    #endregion
}