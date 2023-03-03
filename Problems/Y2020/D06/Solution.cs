using Problems.Y2020.Common;
using Utilities.Extensions;

namespace Problems.Y2020.D06;

/// <summary>
/// Custom Customs: https://adventofcode.com/2020/day/6
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 6;
    
    public override object Run(int part)
    {
        var groupAnswers = ParseGroupAnswers(GetInputLines());
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
    
    private static IEnumerable<IList<string>> ParseGroupAnswers(IEnumerable<string> input)
    {
        var groupAnswers = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                yield return groupAnswers;
                groupAnswers = new List<string>();
                continue;
            }
            
            groupAnswers.Add(line);
        }

        yield return groupAnswers;
    }
}