using Utilities.Extensions;

namespace Solutions.Y2024.D01;

[PuzzleInfo("Historian Hysteria", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var l1 = new List<int>();
        var l2 = new List<int>();

        foreach (var numbers in ParseInputLines(l => l.ParseInts()))
        {
            l1.Add(numbers[0]);
            l2.Add(numbers[1]);
        }

        l1.Sort();
        l2.Sort();

        return part switch
        {
            1 => l1.Zip(l2, (n1, n2) => Math.Abs(n1 - n2)).Sum(),
            2 => l1.Sum(n1 => n1 * l2.Count(n2 => n1 == n2)),
            _ => PuzzleNotSolvedString
        };
    }
}