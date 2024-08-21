namespace Solutions.Y2020.D05;

[PuzzleInfo("Binary Boarding", Topics.BitwiseOperations, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var boardingPasses = GetInputLines();
        return part switch
        {
            1 => boardingPasses.Max(GetSeatId),
            2 => FindMissingSeat(boardingPasses),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindMissingSeat(IEnumerable<string> boardingPasses)
    {
        var orderedSeatIds = boardingPasses
            .Select(GetSeatId)
            .Order()
            .ToList();

        for (var i = 1; i < orderedSeatIds.Count; i++)
        {
            if (orderedSeatIds[i] != orderedSeatIds[i - 1] + 1)
            {
                return orderedSeatIds[i] - 1;
            }
        }

        throw new NoSolutionException();
    }

    private static int GetSeatId(string boardingPass)
    {
        const int numRows = 128;
        const int numCols = 8;
        
        var rowBits = (int)Math.Round(Math.Log2(numRows));
        var row = Convert.ToInt16(boardingPass[..rowBits]
            .Replace(oldChar: 'F', newChar: '0')
            .Replace(oldChar: 'B', newChar: '1'), fromBase: 2);
        var col = Convert.ToInt16(boardingPass[rowBits..]
            .Replace(oldChar: 'L', newChar: '0')
            .Replace(oldChar: 'R', newChar: '1'), fromBase: 2);

        return numCols * row + col;

    }
}