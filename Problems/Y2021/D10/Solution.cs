using Problems.Y2021.Common;

namespace Problems.Y2021.D10;

/// <summary>
/// Syntax Scoring: https://adventofcode.com/2021/day/10
/// </summary>
public class Solution : SolutionBase2021
{
    private static readonly List<SyntaxChecker.Rule> SyntaxRules = new()
    {
        new SyntaxChecker.Rule('(', ')', 3, 1),
        new SyntaxChecker.Rule('[', ']', 57, 2),
        new SyntaxChecker.Rule('{', '}', 1197, 3),
        new SyntaxChecker.Rule('<', '>', 25137, 4),
    };

    public override int Day => 10;
    
    public override object Run(int part)
    {
        var lines = GetInput();
        return part switch
        {
            0 => SumCorruptedLineSyntaxErrors(lines),
            1 => GetMedianCompletionStringScore(lines),
            _ => ProblemNotSolvedString,
        };
    }

    private static long SumCorruptedLineSyntaxErrors(IEnumerable<string> lines)
    {
        var errorPoints = 0L;
        var syntaxChecker = new SyntaxChecker(SyntaxRules);

        void OnSyntaxErrorDetected(long points)
        {
            errorPoints += points;
        }

        syntaxChecker.SyntaxErrorDetected += OnSyntaxErrorDetected;
        syntaxChecker.Evaluate(lines);
        
        return errorPoints;
    }

    private static long GetMedianCompletionStringScore(IEnumerable<string> lines)
    {
        var completionScores = new List<long>();
        var syntaxChecker = new SyntaxChecker(SyntaxRules);

        void OnIncompleteLineDetected(long score)
        {
            completionScores.Add(score);
        }

        syntaxChecker.IncompleteLineDetected += OnIncompleteLineDetected;
        syntaxChecker.Evaluate(lines);

        completionScores.Sort();
        return completionScores[completionScores.Count / 2];
    }
}