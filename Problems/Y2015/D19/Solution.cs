using Problems.Common;

namespace Problems.Y2015.D19;

/// <summary>
/// Medicine for Rudolph: https://adventofcode.com/2015/day/19
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var molecule = input[^1];
        var rules = input[..^2].Select(ParseRule).ToList();

        return part switch
        {
            1 => Combinations(molecule, rules),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int Combinations(string molecule, IEnumerable<Rule> rules)
    {
        var distinct = new HashSet<string>();
        foreach (var rule in rules)
        {
            var yieldLength = rule.Match.Length;
            var partitions = molecule.Length - yieldLength + 1;
            
            for (var i = 0; i < partitions; i++)
            {
                if (molecule[i..(i + yieldLength)] == rule.Match)
                {
                    distinct.Add($"{molecule[..i]}{rule.Yields}{molecule[(i + yieldLength)..]}");
                }
            }
        }
        
        return distinct.Count;
    }
    
    private static Rule ParseRule(string line)
    {
        var tokens = line.Split(separator: " => ");
        return new Rule(Match: tokens[0], Yields: tokens[1]);
    }
    
    private readonly record struct Rule(string Match, string Yields);
}