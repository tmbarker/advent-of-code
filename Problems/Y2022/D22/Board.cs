using System.Text.RegularExpressions;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D22;

public static class Board
{
    private const string InstructionRegex = @"(\d+|[LR])";
    private const string Left = "L";
    private const string Right = "R";
    private const char Free = '.';
    private const char Blocked = '#';
    
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