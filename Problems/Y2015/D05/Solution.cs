using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;

namespace Problems.Y2015.D05;

/// <summary>
/// Doesn't He Have Intern-Elves For This?: https://adventofcode.com/2015/day/5
/// </summary>
[Favourite("Doesn't He Have Intern-Elves For This?", Topics.RegularExpressions, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Regex NiceA = new(@"^(?=.*(.)\1)(?!.*(ab|cd|pq|xy))(.*[aeiou]){3,}");
    private static readonly Regex NiceB = new(@"^(?=.*(.)(.).*\1\2).*(.).\3");

    public override object Run(int part)
    {
        var strings = GetInputLines();
        return part switch
        {
            1 => strings.Count(s => NiceA.IsMatch(s)),
            2 => strings.Count(s => NiceB.IsMatch(s)),
            _ => ProblemNotSolvedString
        };
    }
}