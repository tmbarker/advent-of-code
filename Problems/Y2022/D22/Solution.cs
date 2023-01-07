using Problems.Common;
using Problems.Y2022.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D22;

/// <summary>
/// Monkey Map: https://adventofcode.com/2022/day/22
/// </summary>
public class Solution : SolutionBase2022
{
    public override int Day => 22;
    
    public override object Run(int part)
    {
        MapData.Parse(GetInputLines(), out var board, out var instructions);
        
        return part switch
        {
            0 => ComputePassword(board, instructions, MoveMode.Planar),
            1 => ComputePassword(board, instructions, MoveMode.Cubic),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputePassword(Grid2D<Square> board, IEnumerable<Instruction> instructions, MoveMode mode)
    {
        var pose = new Pose(FindStartPos(board), Vector2D.Right);

        foreach (var instruction in instructions)
        {
            pose = FollowInstruction(pose, board, instruction, mode);
        }

        return ComputePassword(pose, board);
    }

    private static Pose FollowInstruction(Pose pose, Grid2D<Square> board, Instruction instr, MoveMode mode)
    {
        var moveSuccess = true;
        for (var i = 0; i < instr.Steps && moveSuccess; i++)
        {
            moveSuccess = mode == MoveMode.Planar
                ? TryMove2D(pose, board)
                : TryMove3D(pose, board);
        }

        pose.Facing = instr.Rotation * pose.Facing;
        return pose;
    }

    private static bool TryMove2D(Pose pose, Grid2D<Square> board)
    {
        var desiredPos = pose.Pos + pose.Facing;
        var defined = board.IsInDomain(desiredPos);
            
        if (defined && board[desiredPos] == Square.Blocked)
        {
            return false;
        }

        if (defined && board[desiredPos] == Square.Free)
        {
            pose.Pos = desiredPos;
            return true;
        }

        return TryWrap2D(pose, board);
    }
    
    private static bool TryWrap2D(Pose pose, Grid2D<Square> board)
    {
        var targetPos = pose.Pos - pose.Facing;
        while (board.IsInDomain(targetPos) && board[targetPos] != Square.OutOfBounds)
        {
            targetPos -= pose.Facing;
        }

        if (board[targetPos + pose.Facing] != Square.Free)
        {
            return false;
        }
        
        pose.Pos = targetPos + pose.Facing;
        return true;
    }
    
    private static bool TryMove3D(Pose pose, Grid2D<Square> board)
    {
        var naivePos = pose.Pos + pose.Facing;
        var defined = board.IsInDomain(naivePos);

        if (!defined || board[naivePos] == Square.OutOfBounds)
        {
            return MapData.TryMoveBetweenFaces(pose, board);
        }

        if (board[naivePos] == Square.Blocked)
        {
            return false;
        }

        pose.Pos = naivePos;
        return true;
    }

    private static int ComputePassword(Pose pose, Grid2D<Square> board)
    {
        var row = board.Height - pose.Pos.Y;
        var col = pose.Pos.X + 1;
        return MapData.RowMultiplier * row + MapData.ColMultiplier * col + MapData.FacingValues[pose.Facing];
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