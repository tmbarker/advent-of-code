using Problems.Attributes;
using Problems.Common;
using Utilities.Language.ContextFree;

namespace Problems.Y2020.D19;

/// <summary>
/// Monster Messages: https://adventofcode.com/2020/day/19
/// </summary>
[Favourite("Monster Messages", Topics.FormalLanguage, Difficulty.Hard)]
public class Solution : SolutionBase
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
            _ => ProblemNotSolvedString
        };
    }

    private int CountRecognitions(bool useOverrides)
    {
        var input = GetInputLines();
        var sentences = input
            .SkipWhile(line => string.IsNullOrWhiteSpace(line) || !char.IsLetter(line[0]))
            .Select(ParseSentence);
        var productions = input
            .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .SelectMany(line => ParseRule(line, useOverrides));

        var cfgGrammar = new Grammar(StartSymbol, productions);
        var cnfGrammar = CnfConverter.Convert(cfgGrammar);
        
        return sentences.Count(s => CykParser.Recognize(
            grammar: cnfGrammar,
            sentence: s));
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
                yields: alternation.Trim('"').Split(separator: ' ', options: Options));
        }
    }

    private static List<string> ParseSentence(string sentenceStr)
    {
        return sentenceStr.Select(c => c.ToString()).ToList();
    }
}