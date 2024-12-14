using System.Text.RegularExpressions;
using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2021.D14;

[PuzzleInfo("Extended Polymerization", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        Parse(GetInputLines(), out var polymer, out var rules);
        return part switch
        {
            1 => GetMaxExtendedVariance(polymer, rules, steps: 10),
            2 => GetMaxExtendedVariance(polymer, rules, steps: 40),
            _ => PuzzleNotSolvedString
        };
    }

    private static long GetMaxExtendedVariance(List<char> polymer, IList<Rule> rules, int steps)
    {
        var extendedPolymer = ExtendPolymer(polymer, rules, steps);
        var maxVariance = ComputeMaxFrequencyVariance(extendedPolymer, polymer[0], polymer[^1]);

        return maxVariance;
    }
    
    private static long ComputeMaxFrequencyVariance(IDictionary<(char, char), long> pairCounts, char start, char end)
    {
        var doubledFrequencies = new DefaultDict<char, long>(defaultValue: 0L);
        foreach (var ((lhs, rhs), count) in pairCounts)
        {
            doubledFrequencies[lhs] += count;
            doubledFrequencies[rhs] += count;
        }

        //  The only polymer elements which aren't doubled are the LHS of the first pair, and the RHS of the last pair
        // 
        doubledFrequencies[start]++;
        doubledFrequencies[end]++;

        var sortedFrequencies = doubledFrequencies.Values
            .Where(c => c > 0)
            .Select(n => n / 2)
            .Order()
            .ToList();

        return sortedFrequencies[^1] - sortedFrequencies[0];
    }

    private static DefaultDict<(char, char), long> ExtendPolymer(List<char> polymer, IList<Rule> rules, int steps)
    {
        var pairCounts = new DefaultDict<(char, char), long>(defaultValue: 0L);
        for (var i = 0; i < polymer.Count - 1; i++)
        {
            pairCounts[(polymer[i], polymer[i + 1])]++;
        }

        for (var i = 0; i < steps; i++)
        {
            ExtendPolymer(pairCounts, rules);
        }

        return pairCounts;
    }

    private static void ExtendPolymer(DefaultDict<(char, char), long> pairCounts, IList<Rule> rules)
    {
        var deltas = new DefaultDict<(char, char), long>(defaultValue: 0L);
        foreach (var (lhs, rhs) in pairCounts.Keys.Freeze())
        {
            foreach (var rule in rules.Where(rule => rule.Lhs == lhs && rule.Rhs == rhs))
            {
                deltas[rule.MatchKey] -= pairCounts[rule.MatchKey];
                deltas[(lhs, rule.Insert)] += pairCounts[rule.MatchKey];
                deltas[(rule.Insert, rhs)] += pairCounts[rule.MatchKey];
            }
        }

        foreach (var (pairKey, delta) in deltas)
        {
            pairCounts[pairKey] += delta;
        }
    }

    private static void Parse(string[] input, out List<char> polymer, out List<Rule> rules)
    {
        polymer = [..input[0]];
        rules = input.Skip(2).Select(ParseRule).ToList();
    }

    private static Rule ParseRule(string line)
    {
        var elements = Regex.Match(line, pattern: @"(.)(.) -> (.)");
        return new Rule
        {
            Lhs = elements.Groups[1].Value[0],
            Rhs = elements.Groups[2].Value[0],
            Insert = elements.Groups[3].Value[0]
        };
    }
}