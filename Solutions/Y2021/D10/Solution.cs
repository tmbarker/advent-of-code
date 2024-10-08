namespace Solutions.Y2021.D10;

[PuzzleInfo("Syntax Scoring", Topics.StringParsing, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly List<SyntaxChecker.Rule> SyntaxRules =
    [
        new SyntaxChecker.Rule(openWith: '(', closeWith: ')', errorPoints:     3, completionPoints: 1),
        new SyntaxChecker.Rule(openWith: '[', closeWith: ']', errorPoints:    57, completionPoints: 2),
        new SyntaxChecker.Rule(openWith: '{', closeWith: '}', errorPoints:  1197, completionPoints: 3),
        new SyntaxChecker.Rule(openWith: '<', closeWith: '>', errorPoints: 25137, completionPoints: 4)
    ];
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        return part switch
        {
            1 => SumCorruptedLineSyntaxErrors(lines),
            2 => GetMedianCompletionStringScore(lines),
            _ => PuzzleNotSolvedString
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