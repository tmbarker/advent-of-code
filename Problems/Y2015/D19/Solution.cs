using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;

namespace Problems.Y2015.D19;

/// <summary>
/// Medicine for Rudolph: https://adventofcode.com/2015/day/19
/// </summary>
[Favourite("Medicine for Rudolph", Topics.RegularExpressions, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountProductions(),
            2 => GetMinDerivationDepth(),
            _ => ProblemNotSolvedString
        };
    }
    
    private int CountProductions()
    {
        var input = GetInputLines();
        var molecule = input[^1];
        var rules = input[..^2].Select(ParseRule).ToList();
        var productions = new HashSet<string>();
        
        foreach (var rule in rules)
        {
            var yieldLength = rule.Match.Length;
            var partitions = molecule.Length - yieldLength + 1;
            
            for (var i = 0; i < partitions; i++)
            {
                if (molecule[i..(i + yieldLength)] == rule.Match)
                {
                    productions.Add($"{molecule[..i]}{rule.Yields}{molecule[(i + yieldLength)..]}");
                }
            }
        }
        
        return productions.Count;
    }

    private int GetMinDerivationDepth()
    {
        //  Examining the input reveals that these production rules constitute a context free grammar (CNF). Trying
        //  to naively use BFS or DFS to evaluate all paths in the parse tree is unfeasible due to the exponential
        //  time complexity. Also, the rules are not in CNF, meaning implementing something like CYK would require
        //  manually translating the production rules.
        //
        //  This solution works by observing that the 'Rn', 'Ar', and 'Y' terminals are structured such that greedy
        //  right hand replacement will always unambiguously yield the starting variable. 
        //
        var input = GetInputLines();
        var molecule = string.Concat(input[^1].Reverse());
        var reverseRules = new Dictionary<string, string>();

        foreach (var line in input[..^2])
        {
            var match = Regex.Match(line, @"(?<M>\w+)\s=>\s(?<Y>\w+)");
            var a = string.Concat(match.Groups["M"].Value.Reverse());
            var b = string.Concat(match.Groups["Y"].Value.Reverse());
            reverseRules[b] = a;
        }

        var alternation = $"(?<E>{string.Join('|', reverseRules.Keys)})";
        var count = 0;

        while (molecule != "e")
        {
            var match = Regex.Match(molecule, alternation);
            var element = match.Groups["E"].Value;
            var replace = new Regex(element);
            
            molecule = replace.Replace(molecule, reverseRules[element], count: 1);
            count++;
        }

        return count;
    }
    
    private static Rule ParseRule(string line)
    {
        var tokens = line.Split(separator: " => ");
        return new Rule(Match: tokens[0], Yields: tokens[1]);
    }
    
    private readonly record struct Rule(string Match, string Yields);
}