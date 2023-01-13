using Problems.Common;
using Problems.Y2020.Common;

namespace Problems.Y2020.D05;

/// <summary>
/// Binary Boarding: https://adventofcode.com/2020/day/5
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 5;
    
    public override object Run(int part)
    {
        var boardingPasses = GetInputLines();
        return part switch
        {
            0 => boardingPasses.Max(GetSeatId),
            1 => FindMissingSeat(boardingPasses),
            _ => ProblemNotSolvedString,
        };
    }

    private static int FindMissingSeat(IEnumerable<string> boardingPasses)
    {
        var orderedSeatIds = boardingPasses
            .Select(GetSeatId)
            .OrderBy(id => id)
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
            .Replace('F', '0')
            .Replace('B', '1'), 2);
        var col = Convert.ToInt16(boardingPass[rowBits..]
            .Replace('L', '0')
            .Replace('R', '1'), 2);

        return numCols * row + col;

    }
}