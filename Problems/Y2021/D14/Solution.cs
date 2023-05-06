using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2021.D14;

/// <summary>
/// Extended Polymerization: https://adventofcode.com/2021/day/14/input
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        Parse(GetInputLines(), out var polymer, out var rules);
        return part switch
        {
            1 => GetMaxExtendedVariance(polymer, rules, steps: 10),
            2 => GetMaxExtendedVariance(polymer, rules, steps: 40),
            _ => ProblemNotSolvedString
        };
    }

    private static long GetMaxExtendedVariance(IList<char> polymer, IList<Rule> rules, int steps)
    {
        var extendedPolymer = ExtendPolymer(polymer, rules, steps);
        var maxVariance = ComputeMaxFrequencyVariance(extendedPolymer, polymer.First(), polymer.Last());

        return maxVariance;
    }
    
    private static long ComputeMaxFrequencyVariance(Dictionary<(char, char), long> pairCounts, char pStart, char pEnd)
    {
        var doubledFrequencies = new Dictionary<char, long>();
        foreach (var ((lhs, rhs), count) in pairCounts)
        {
            doubledFrequencies.EnsureContainsKey(lhs);
            doubledFrequencies.EnsureContainsKey(rhs);
            doubledFrequencies[lhs] += count;
            doubledFrequencies[rhs] += count;
        }

        //  The only polymer elements which aren't doubled are the LHS of the first pair, and the RHS of the last pair
        // 
        doubledFrequencies[pStart]++;
        doubledFrequencies[pEnd]++;

        var sortedFrequencies = doubledFrequencies.Values
            .Where(c => c > 0)
            .Select(n => n/2)
            .Order()
            .ToList();

        return sortedFrequencies.Last() - sortedFrequencies.First();
    }

    private static Dictionary<(char, char), long> ExtendPolymer(IList<char> polymer, IList<Rule> rules, int steps)
    {
        var pairCounts = new Dictionary<(char, char), long>();
        for (var i = 0; i < polymer.Count - 1; i++)
        {
            var key = (polymer[i], polymer[i + 1]);
            pairCounts.EnsureContainsKey(key);
            pairCounts[key]++;
        }

        for (var i = 0; i < steps; i++)
        {
            ExtendPolymer(pairCounts, rules);
        }

        return pairCounts;
    }

    private static void ExtendPolymer(Dictionary<(char, char), long> pairCounts, IList<Rule> rules)
    {
        var deltas = new Dictionary<(char, char), long>();
        foreach (var (lhs, rhs) in pairCounts.Keys.Freeze())
        {
            foreach (var rule in rules.Where(rule => rule.Lhs == lhs && rule.Rhs == rhs))
            {
                var lPairKey = (lhs, rule.Insert);
                var rPairKey = (rule.Insert, rhs);

                deltas.EnsureContainsKey(rule.MatchKey);
                deltas.EnsureContainsKey(lPairKey);
                deltas.EnsureContainsKey(rPairKey);

                deltas[rule.MatchKey] -= pairCounts[rule.MatchKey];
                deltas[lPairKey] += pairCounts[rule.MatchKey];
                deltas[rPairKey] += pairCounts[rule.MatchKey];
            }
        }

        foreach (var (pairKey, delta) in deltas)
        {
            pairCounts.EnsureContainsKey(pairKey);
            pairCounts[pairKey] += delta;
        }
    }

    private static void Parse(IList<string> input, out List<char> polymer, out List<Rule> rules)
    {
        polymer = new List<char>(input[0]);
        rules = input.Skip(2).Select(ParseRule).ToList();
    }

    private static Rule ParseRule(string line)
    {
        var elements = Regex.Match(line, @"(.)(.) -> (.)");
        return new Rule
        {
            Lhs = elements.Groups[1].Value[0],
            Rhs = elements.Groups[2].Value[0],
            Insert = elements.Groups[3].Value[0]
        };
    }
}