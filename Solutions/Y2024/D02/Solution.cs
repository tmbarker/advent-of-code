using Utilities.Extensions;

namespace Solutions.Y2024.D02;

[PuzzleInfo("Red-Nosed Reports", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var reports = ParseInputLines(l => l.ParseInts());
        return part switch
        {
            1 => reports.Count(IsSafe),
            2 => reports.Select(Permute).Count(set => set.Any(IsSafe)),
            _ => PuzzleNotSolvedString
        };
    }

    private static bool IsSafe(IList<int> report)
    {
        var sign = Math.Sign(report[1] - report[0]);
        return report
            .Zip(report.Skip(1), (n1, n2) => n2 - n1)
            .All(d => Math.Sign(d) == sign && Math.Abs(d) is >= 1 and <= 3);
    }

    private static IEnumerable<IList<int>> Permute(IList<int> report)
    {
        for (var i = 0; i < report.Count; i++)
        {
            var variant = new List<int>(report);
            variant.RemoveAt(i);
            yield return variant;
        }
    }
}