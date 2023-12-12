using Utilities.Extensions;

namespace Solutions.Y2020.D06;

/// <summary>
/// Custom Customs: https://adventofcode.com/2020/day/6
/// </summary>
[PuzzleInfo("Custom Customs", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var groupAnswers = lines.ChunkBy(line => !string.IsNullOrWhiteSpace(line));
        
        return part switch
        {
            1 => groupAnswers.Sum(GetUniqueGroupAnswers),
            2 => groupAnswers.Sum(GetUnanimousGroupAnswers),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetUniqueGroupAnswers(IList<string> groupAnswers)
    {
        return groupAnswers.SelectMany(g => g).Distinct().Count();
    }

    private static int GetUnanimousGroupAnswers(IList<string> groupAnswers)
    {
        return groupAnswers.IntersectAll().Count;
    }
}