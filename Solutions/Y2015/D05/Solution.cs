using System.Text.RegularExpressions;

namespace Solutions.Y2015.D05;

[PuzzleInfo("Doesn't He Have Intern-Elves For This?", Topics.RegularExpressions, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Regex NiceA = new(pattern:@"^(?=.*(.)\1)(?!.*(ab|cd|pq|xy))(.*[aeiou]){3,}");
    private static readonly Regex NiceB = new(pattern:@"^(?=.*(.)(.).*\1\2).*(.).\3");

    public override object Run(int part)
    {
        var strings = GetInputLines();
        return part switch
        {
            1 => strings.Count(NiceA.IsMatch),
            2 => strings.Count(NiceB.IsMatch),
            _ => PuzzleNotSolvedString
        };
    }
}