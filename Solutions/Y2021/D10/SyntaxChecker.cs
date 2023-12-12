namespace Solutions.Y2021.D10;

public sealed class SyntaxChecker
{
    private const long CompletionCharacterMultiplier = 5;
    
    private readonly Dictionary<char, Rule> _openWithRuleMap = new();
    private readonly Dictionary<char, Rule> _closeWithRuleMap = new();
    
    public readonly struct Rule
    {
        public Rule(char openWith, char closeWith, int errorPoints, int completionPoints)
        {
            OpenWith = openWith;
            CloseWith = closeWith;
            ErrorPoints = errorPoints;
            CompletionPoints = completionPoints;
        }
    
        public char OpenWith { get; }
        public char CloseWith { get; }
        public int ErrorPoints { get; }
        public int CompletionPoints { get; }
    }

    public event Action<long>? SyntaxErrorDetected;
    public event Action<long>? IncompleteLineDetected;

    public SyntaxChecker(IEnumerable<Rule> rules)
    {
        foreach (var rule in rules)
        {
            _openWithRuleMap.Add(rule.OpenWith, rule);
            _closeWithRuleMap.Add(rule.CloseWith, rule);
        }
    }

    public void Evaluate(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            Evaluate(line);
        }
    }

    private void Evaluate(string line)
    {
        var stack = new Stack<char>();
        foreach (var c in line)
        {
            if (_openWithRuleMap.TryGetValue(c, out var rule))
            {
                stack.Push(rule.CloseWith);
                continue;
            }

            if (!_closeWithRuleMap.TryGetValue(c, out rule))
            {
                throw new NoSolutionException();
            }

            if (stack.Count != 0 && stack.Pop() == c)
            {
                continue;
            }
            
            RaiseSyntaxErrorDetected(rule);
            return;
        }

        if (stack.Count != 0)
        {
            RaiseIncompleteLineDetected(string.Join(string.Empty, stack));
        }
    }

    private long ScoreCompletionString(string completionString)
    {
        var points = 0L;
        foreach (var c in completionString)
        {
            points *= CompletionCharacterMultiplier;
            points += _closeWithRuleMap[c].CompletionPoints;
        }
        return points;
    }
    
    private void RaiseSyntaxErrorDetected(Rule brokenRule)
    {
        SyntaxErrorDetected?.Invoke(brokenRule.ErrorPoints);
    }

    private void RaiseIncompleteLineDetected(string completionString)
    {
        IncompleteLineDetected?.Invoke(ScoreCompletionString(completionString));
    }
}