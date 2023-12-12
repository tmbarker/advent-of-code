namespace Problems.Y2018.D12;

[PuzzleInfo("Subterranean Sustainability", Topics.BitwiseOperations, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        Input.Parse(
            input: GetInputLines(),
            out var state,
            out var rules);
        
        return part switch
        {
            1 => Generate(state, rules, generations: 20L),
            2 => Generate(state, rules, generations: 50000000000L),
            _ => ProblemNotSolvedString
        };
    }

    private static long Generate(HashSet<long> state, IReadOnlyDictionary<uint, bool> rules, long generations)
    {
        var (step, sum, inc) = FindSteadyState(state, rules);
        if (generations >= step)
        {
            return sum + (generations - step) * inc;
        }
        
        for (var i = 0L; i < generations; i++)
        {
            state = Step(state, rules);
        }

        return state.Sum();
    }

    private static (long Step, long Sum, long Inc) FindSteadyState(HashSet<long> state, IReadOnlyDictionary<uint, bool> rules)
    {
        var step = 0;
        var memo = new Dictionary<string, int>();
        var key = FormStateKey(state);

        while (!memo.ContainsKey(key))
        {
            memo.Add(key, step++);
            state = Step(state, rules);
            key = FormStateKey(state);
        }

        var s1 = state.Sum();
        var s2 = Step(state, rules).Sum();

        return (step, s1, s2 - s1);
    }

    private static HashSet<long> Step(IReadOnlySet<long> state, IReadOnlyDictionary<uint, bool> rules)
    {
        var next = new HashSet<long>();
        var min = state.Min() - 2;
        var max = state.Max() + 2;

        for (var i = min; i <= max; i++)
        {
            var mask = 0U;
            for (var j = 0; j < 5; j++)
            {
                if (state.Contains(i + j - 2))
                {
                    mask |= 1U << j;
                }
            }

            if (rules.ContainsKey(mask) && rules[mask])
            {
                next.Add(i);
            }
        }

        return next;
    }
    
    private static string FormStateKey(ICollection<long> state)
    {
        var min = state.Min();
        return string.Join(',', state.Select(p => p - min));
    }
}