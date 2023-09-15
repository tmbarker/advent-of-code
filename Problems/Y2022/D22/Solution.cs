using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D22;

/// <summary>
/// Monkey Map: https://adventofcode.com/2022/day/22
/// </summary>
public class Solution : SolutionBase
{
    private delegate bool MoveHandler(Pose2D pose, Grid2D<Square> board, out Pose2D result);
    private static readonly Dictionary<MoveMode, MoveHandler> MoveHandlers = new()
    {
        { MoveMode.Planar, TryMove2D },
        { MoveMode.Cubic,  TryMove3D }
    };
    
    public override object Run(int part)
    {
        MapData.Parse(GetInputLines(), out var board, out var instructions);
        
        return part switch
        {
            1 => ComputePassword(board, instructions, MoveMode.Planar),
            2 => ComputePassword(board, instructions, MoveMode.Cubic),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputePassword(Grid2D<Square> board, IEnumerable<Instruction> instructions, MoveMode mode)
    {
        var pose = new Pose2D(
            pos: FindStartPos(board),
            face: Vector2D.Right);

        pose = instructions.Aggregate(
            seed: pose,
            func: (current, instruction) => FollowInstruction(current, board, instruction, mode));

        return ComputePassword(pose, board);
    }

    private static Pose2D FollowInstruction(Pose2D pose, Grid2D<Square> board, Instruction instr, MoveMode mode)
    {
        var moveSuccess = true;
        for (var i = 0; i < instr.Steps && moveSuccess; i++)
        {
            moveSuccess = MoveHandlers[mode](pose, board, out var result);
            if (moveSuccess)
            {
                pose = result;
            }
        }

        return pose.Turn(instr.Rotation);
    }

    private static bool TryMove2D(Pose2D pose, Grid2D<Square> board, out Pose2D result)
    {
        if (board.IsInDomain(pose.Ahead) && board[pose.Ahead] == Square.Blocked)
        {
            result = default;
            return false;
        }

        if (board.IsInDomain(pose.Ahead) && board[pose.Ahead] == Square.Free)
        {
            result = pose.Step();
            return true;
        }

        return TryWrap2D(pose, board, out result);
    }
    
    private static bool TryWrap2D(Pose2D pose, Grid2D<Square> board, out Pose2D result)
    {
        var targetPos = pose.Pos - pose.Face;
        while (board.IsInDomain(targetPos) && board[targetPos] != Square.OutOfBounds)
        {
            targetPos -= pose.Face;
        }

        if (board[targetPos + pose.Face] != Square.Free)
        {
            result = default;
            return false;
        }

        result = new Pose2D(pos: targetPos + pose.Face, face: pose.Face);
        return true;
    }
    
    private static bool TryMove3D(Pose2D pose, Grid2D<Square> board, out Pose2D result)
    {
        if (!board.IsInDomain(pose.Ahead) || board[pose.Ahead] == Square.OutOfBounds)
        {
            return MapData.TryMoveBetweenFaces(pose, board, out result);
        }

        if (board[pose.Ahead] == Square.Blocked)
        {
            result = default;
            return false;
        }

        result = pose.Step();
        return true;
    }

    private static int ComputePassword(Pose2D pose, Grid2D<Square> board)
    {
        var row = board.Height - pose.Pos.Y;
        var col = pose.Pos.X + 1;
        return MapData.RowFactor * row + MapData.ColFactor * col + MapData.FacingOffset[pose.Face];
    }
    
    private static Vector2D FindStartPos(Grid2D<Square> board)
    {
        for (var y = board.Height - 1; y >= 0; y--)
        for (var x = 0; x < board.Width; x++)
        {
            if (board[x, y] == Square.Free)
            {
                return new Vector2D(x, y);
            }
        }

        throw new NoSolutionException();
    }
}