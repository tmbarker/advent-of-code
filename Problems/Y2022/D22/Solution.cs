using Problems.Common;
using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D22;

/// <summary>
/// Monkey Map: https://adventofcode.com/2022/day/22
/// </summary>
public class Solution : SolutionBase2022
{
    private const int RowScoreMultiplier = 1000;
    private const int ColScoreMultiplier = 4;
    
    private static readonly Dictionary<Vector2D, int> FacingScores = new()
    {
        { Vector2D.Right, 0 },
        { Vector2D.Down, 1 },
        { Vector2D.Left, 2 },
        { Vector2D.Up, 3 },
    };

    public override int Day => 22;
    
    public override object Run(int part)
    {
        Board.Parse(GetInput(), out var board, out var instructions);
        return part switch
        {
            0 => ComputePassword(board, instructions),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputePassword(Grid2D<Square> board, IEnumerable<Instruction> instructions)
    {
        var facing = Vector2D.Right;
        var pos = FindStart(board);
        
        foreach (var instruction in instructions)
        {
            (pos, facing) = FollowInstruction(pos, facing, board, instruction);
        }
        
        var scoringRow = board.Height - pos.Y;
        var scoringCol = pos.X + 1;
        var score = RowScoreMultiplier * scoringRow + ColScoreMultiplier * scoringCol + FacingScores[facing];

        return score;
    }

    private static (Vector2D pos, Vector2D facing) FollowInstruction(Vector2D pos, Vector2D facing, Grid2D<Square> board, Instruction instr)
    {
        for (var i = 0; i < instr.Steps; i++)
        {
            var desired = pos + facing;
            var defined = board.IsInDomain(desired);
            
            if (defined && board[desired] == Square.Blocked)
            {
                break;
            }

            if (defined && board[desired] == Square.Free)
            {
                pos = desired;
                continue;
            }
            
            if (TryFindWrapPos(pos, facing, board, out var wrapPos))
            {
                pos = wrapPos;
                continue;
            }
            
            break;
        }

        facing = instr.Rotation * facing;
        return (pos, facing);
    }

    private static bool TryFindWrapPos(Vector2D pos, Vector2D facing, Grid2D<Square> board, out Vector2D wrapPos)
    {
        wrapPos = pos - facing;
        while (board.IsInDomain(wrapPos) && board[wrapPos] != Square.OutOfBounds)
        {
            wrapPos -= facing;
        }

        if (board[wrapPos + facing] != Square.Free)
        {
            return false;
        }
        
        wrapPos += facing;
        return true;

    }

    private static Vector2D FindStart(Grid2D<Square> board)
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