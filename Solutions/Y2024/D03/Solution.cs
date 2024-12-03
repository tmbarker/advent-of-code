using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2024.D03;

[PuzzleInfo("Mull It Over", Topics.RegularExpressions, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var text = GetInputText();
        return part switch
        {
            1 => SumNaive(text),
            2 => SumContextAware(text),
            _ => PuzzleNotSolvedString
        };
    }

    private static int SumNaive(string text)
    {
        var matches = Regex.Matches(text, @"mul\((?:\d){1,3},(?:\d){1,3}\)");
        return matches
            .Select(match => match.Value.ParseInts())
            .Select(nums => nums[0] * nums[1])
            .Sum();
    }
    
    private static int SumContextAware(string text)
    {
        var runs = Regex.Split(text, @"(do\(\))|(don't\(\))");
        return runs
            .Where((_, i) => i == 0 || runs[i - 1] == "do()")
            .Sum(SumNaive);
    }
}