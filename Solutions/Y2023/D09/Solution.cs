using Utilities.Extensions;

namespace Solutions.Y2023.D09;

[PuzzleInfo("Mirage Maintenance", Topics.Math, Difficulty.Easy)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        return part switch
        {
            1 => input.Sum(line => Extrapolate(report: line, forwards: true)),
            2 => input.Sum(line => Extrapolate(report: line, forwards: false)),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Extrapolate(string report, bool forwards)
    {
        var values = report.ParseInts();
        var initial = forwards
            ? values
            : values.Reverse();
        
        IList<int[]> sequences = [initial.ToArray()];
        while (sequences[^1].Any(val => val != 0))
        {
            sequences.Add(item: sequences[^1]
                .Skip(1)
                .Select((val, i) => val - sequences[^1][i])
                .ToArray());
        }
        
        return sequences
            .Reverse()
            .Skip(1)
            .Aggregate(seed: 0, func: (n, seq) => n + seq[^1]);
    }
}