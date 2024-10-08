using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2020.D02;

[PuzzleInfo("Password Philosophy", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Policy(int N1, int N2, char Letter, string Password);
    
    public override object Run(int part)
    {
        var policies = ParseInputLines(parseFunc: ParsePolicy);
        return part switch
        {
            1 => CountValidPoliciesWithRange(policies),
            2 => CountValidPoliciesWithIndices(policies),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountValidPoliciesWithRange(IEnumerable<Policy> policies)
    {
        return (
            from policy in policies 
            let count = policy.Password.Count(c => c == policy.Letter) 
            where count >= policy.N1 && count <= policy.N2 
            select policy).Count();
    }
    
    private static int CountValidPoliciesWithIndices(IEnumerable<Policy> policies)
    {
        return (
            from policy in policies
            where policy.Password[policy.N1 - 1] == policy.Letter ^ policy.Password[policy.N2 - 1] == policy.Letter  
            select policy).Count();
    }

    private static Policy ParsePolicy(string line)
    {
        var match = Regex.Match(line, @"(\d+)-(\d+) ([a-z]): (.*)");
        return new Policy(
            N1: match.Groups[1].ParseInt(),
            N2: match.Groups[2].ParseInt(),
            Letter: match.Groups[3].Value[0],
            Password: match.Groups[4].Value);
    }
}