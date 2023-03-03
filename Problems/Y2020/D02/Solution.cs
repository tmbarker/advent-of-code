using System.Text.RegularExpressions;
using Problems.Y2020.Common;

namespace Problems.Y2020.D02;

/// <summary>
/// Password Philosophy: https://adventofcode.com/2020/day/2
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 2;
    
    public override object Run(int part)
    {
        var policies = ParseInputLines(parseFunc: ParsePolicy);
        return part switch
        {
            1 => CountValidPoliciesWithRange(policies),
            2 => CountValidPoliciesWithIndices(policies),
            _ => ProblemNotSolvedString
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
        var matches = Regex.Match(line, @"(\d+)-(\d+) ([a-z]): (.*)");
        return new Policy(
            N1: int.Parse(matches.Groups[1].Value),
            N2: int.Parse(matches.Groups[2].Value),
            Letter: matches.Groups[3].Value[0],
            Password: matches.Groups[4].Value);
    }
}