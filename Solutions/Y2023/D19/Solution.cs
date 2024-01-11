using System.Text.RegularExpressions;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Numerics;

namespace Solutions.Y2023.D19;

using Ratings   = Dictionary<string, Range<long>>;
using Workflows = DefaultDict<string, List<Rule>>;

[PuzzleInfo("Aplenty", Topics.StringParsing|Topics.Math, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var chunks = GetInputLines()
            .ChunkByNonEmpty()
            .ToArray();
        
        var workflows = ParseWorkflows(lines: chunks[0]);
        var parts = ParseParts(lines: chunks[1]);
        
        return part switch
        {
            1 => SumAccepted(parts, workflows),
            2 => CountCombinations(workflows),
            _ => ProblemNotSolvedString
        };
    }

    private static long SumAccepted(IEnumerable<Ratings> parts, Workflows workflows)
    {
        return parts
            .Where(part => CountAccepted(id: "in", workflows, part) > 0)
            .Sum(part => part.Values.Sum(range => range.Single()));
    }
    
    private static long CountCombinations(Workflows workflows)
    {
        return CountAccepted(id: "in", workflows, ratings: new Ratings
        {
            { "x", new Range<long>(Min: 1L, Max: 4000L) },
            { "m", new Range<long>(Min: 1L, Max: 4000L) },
            { "a", new Range<long>(Min: 1L, Max: 4000L) },
            { "s", new Range<long>(Min: 1L, Max: 4000L) }
        });
    }

    private static long CountAccepted(string id, Workflows workflows, Ratings ratings)
    {
        switch (id)
        {
            case "A":
                return ratings.Values.Aggregate(seed: 1L, func: (product, range) => product * range.Length);
            case "R":
                return 0;
        }
        
        foreach (var (lhs, op, rhs, next) in workflows[id])
        {
            var range = ratings[lhs];
            var pass = default(Range<long>);
            var fail = default(Range<long>);
            
            switch (op)
            {
                case "<" when range.Min >= rhs:
                case ">" when range.Max <= rhs:
                    continue;
                case "<" when range.Max < rhs:
                case ">" when range.Min > rhs:
                    return CountAccepted(id: next, workflows, ratings);
                case ">":
                    fail = new Range<long>(Min: range.Min, Max: rhs);
                    pass = new Range<long>(Min: rhs + 1,   Max: range.Max);
                    break;
                case "<":
                    pass = new Range<long>(Min: range.Min, Max: rhs - 1);
                    fail = new Range<long>(Min: rhs,       Max: range.Max);
                    break;
            }
            
            return CountAccepted(id: next, workflows, ratings: BranchRatings(ratings, category: lhs, range: pass)) +
                   CountAccepted(id: id,   workflows, ratings: BranchRatings(ratings, category: lhs, range: fail));
        }

        throw new NoSolutionException();
    }

    private static Ratings BranchRatings(Ratings nominal, string category, Range<long> range)
    {
        return new Ratings(collection: nominal)
        {
            [category] = range
        };
    }

    private static Workflows ParseWorkflows(IEnumerable<string> lines)
    {
        var workflows = new Workflows(defaultSelector: _ => []);
        foreach (var line in lines)
        {
            var elements = line.Split(separator: ['{', '}']);
            var id = elements[0];
            var rules = elements[1].Split(separator: ',');

            for (var i = 0; i < rules.Length - 1; i++)
            {
                var m = Regex.Match(input: rules[i], pattern: @"(?<L>[xmas])(?<O>[\<\>])(?<R>\d+)\:(?<N>[a-zA-Z]+)$");
                workflows[id].Add(item: new Rule(
                    Lhs:  m.Groups["L"].Value,
                    Op:   m.Groups["O"].Value,
                    Rhs:  m.Groups["R"].ParseLong(),
                    Next: m.Groups["N"].Value));
            }
            
            workflows[id].Add(item: new Rule(Lhs: "x", Op: ">", Rhs: 0, Next: rules[^1]));
        }
        return workflows;
    }

    private static IEnumerable<Ratings> ParseParts(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var longs = line.ParseLongs();
            yield return new Ratings
            {
                {"x", Range<long>.Single(longs[0])},
                {"m", Range<long>.Single(longs[1])},
                {"a", Range<long>.Single(longs[2])},
                {"s", Range<long>.Single(longs[3])}
            };
        }
    }
}