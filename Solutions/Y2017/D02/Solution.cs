using Utilities.Extensions;

namespace Solutions.Y2017.D02;

[PuzzleInfo("Corruption Checksum", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var table = ParseInputLines(parseFunc: StringExtensions.ParseInts);
        return part switch
        {
            1 => GetChecksum(table),
            2 => GetDivisorSum(table),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetChecksum(IEnumerable<IList<int>> table)
    {
        return table.Sum(row => row.Max() - row.Min());
    }

    private static int GetDivisorSum(IEnumerable<IList<int>> table)
    {
        return table.Sum(row =>
        {
            for (var i = 0; i < row.Count; i++)
            for (var j = 0; j < row.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (row[i] % row[j] == 0)
                {
                    return row[i] / row[j];
                }
            }

            throw new NoSolutionException();
        });
    }
}