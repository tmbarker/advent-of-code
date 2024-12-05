using Utilities.Extensions;
using Utilities.Language.ContextFree;

namespace Solutions.Y2020.D19;

[PuzzleInfo("Monster Messages", Topics.FormalLanguage, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const string StartSymbol = "0";
    private const StringSplitOptions Options = StringSplitOptions.TrimEntries;

    private static readonly Dictionary<string, string> Overrides = new()
    {
        { "8", "42 | 42 8" },
        { "11", "42 31 | 42 11 31" }
    };

    public override object Run(int part)
    {
        return part switch
        {
            1 => CountRecognitions(useOverrides: false),
            2 => CountRecognitions(useOverrides: true),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountRecognitions(bool useOverrides)
    {
        var input = GetInputLines()
            .ChunkByNonEmpty();
        var productions = input[0]
            .SelectMany(line => ParseRule(line, useOverrides));
        var sentences = input[1]
            .Select(Tokenize)
            .AsParallel();

        var cfg = new Grammar(StartSymbol, productions);
        var cnf = CnfConverter.Convert(cfg);
        var cyk = new CykParser(cnf);
        
        return sentences.Count(cyk.Recognize);
    }

    private static IEnumerable<Production> ParseRule(string ruleStr, bool useOverrides)
    {
        var elements = ruleStr.Split(separator: ':', options: Options);
        var nonTerminal = elements[0];
        var yieldStr = useOverrides && Overrides.TryGetValue(nonTerminal, out var @override)
            ? @override
            : elements[1];

        foreach (var alternation in yieldStr.Split(separator:'|', options: Options))
        {
            yield return new Production(
                nonTerminal: nonTerminal,
                yields: alternation.Trim(trimChar: '"').Split(separator: ' ', options: Options));
        }
    }

    private static List<string> Tokenize(string sentence)
    {
        return sentence.Select(c => c.ToString()).ToList();
    }
}