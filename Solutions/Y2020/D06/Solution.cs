using Utilities.Extensions;

namespace Solutions.Y2020.D06;

[PuzzleInfo("Custom Customs", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var groupAnswers = ChunkInputByNonEmpty();
        return part switch
        {
            1 => groupAnswers.Sum(GetUniqueGroupAnswers),
            2 => groupAnswers.Sum(GetUnanimousGroupAnswers),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetUniqueGroupAnswers(string[] groupAnswers)
    {
        return groupAnswers.SelectMany(g => g).Distinct().Count();
    }

    private static int GetUnanimousGroupAnswers(string[] groupAnswers)
    {
        return groupAnswers.IntersectAll().Count;
    }
}