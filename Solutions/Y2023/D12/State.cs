namespace Solutions.Y2023.D12;

public readonly record struct State(string Pattern, int Pos, char Current, bool RunActive, int RunIndex, int RunLength)
{
    public static State Initial(string pattern)
    {
        return new State(Pattern: pattern, Pos: 0, Current: pattern[0], RunActive: false, RunIndex: -1, RunLength: 0);
    }

    public State Branch(char c)
    {
        return this with { Current = c };
    }

    public State Consume()
    {
        return Current == '.'
            ? AdvanceWorking()
            : AdvanceDamaged();
    }
    
    private State AdvanceWorking()
    {
        var c = GetTokenOrDefault(Pos + 1);
        return new State(Pattern: Pattern, Pos: Pos + 1, Current: c, RunActive: false, RunIndex: RunIndex, RunLength: 0);
    }
    
    private State AdvanceDamaged()
    {
        var c = GetTokenOrDefault(Pos + 1);
        return RunActive
            ? new State(Pattern: Pattern, Pos: Pos + 1, Current: c, RunActive: true, RunIndex: RunIndex,     RunLength: RunLength + 1)
            : new State(Pattern: Pattern, Pos: Pos + 1, Current: c, RunActive: true, RunIndex: RunIndex + 1, RunLength: 1);
    }

    private char GetTokenOrDefault(int pos)
    {
        return pos < Pattern.Length
            ? Pattern[pos]
            : '\0';
    }
}