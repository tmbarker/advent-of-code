namespace Solutions.Y2023.D12;

public readonly struct State : IEquatable<State>
{
    private readonly string _pattern;
    
    public char Current { get; }
    public bool InRun { get; }
    public int Pos { get; }
    public int RunIndex { get; }
    public int RunLength { get; }

    private State(string pattern, char current, bool inRun, int pos, int runIndex, int runLength)
    {
        _pattern = pattern;
        Current = current;
        InRun = inRun;
        Pos = pos;
        RunIndex = runIndex;
        RunLength = runLength;
    }

    public static State Initial(string pattern)
    {
        return new State(pattern: pattern, current: pattern[0], inRun: false, pos: 0, runIndex: -1, runLength: 0);
    }

    public State Replace(char c)
    {
        return new State(pattern: _pattern, current: c, inRun: InRun, pos: Pos, runIndex: RunIndex, runLength: RunLength);
    }

    public State Consume()
    {
        return Current == '.'
            ? AdvanceWorking()
            : AdvanceDamaged();
    }
    
    private State AdvanceWorking()
    {
        var c = GetNextTokenOrDefault();
        return new State(pattern: _pattern, current: c, inRun: false, pos: Pos + 1, runIndex: RunIndex, runLength: 0);
    }
    
    private State AdvanceDamaged()
    {
        var c = GetNextTokenOrDefault();
        return InRun
            ? new State(pattern: _pattern, current: c, inRun: true, pos: Pos + 1, runIndex: RunIndex, runLength: RunLength + 1)
            : new State(pattern: _pattern, current: c, inRun: true, pos: Pos + 1, runIndex: RunIndex + 1, runLength: 1);
    }

    private char GetNextTokenOrDefault()
    {
        var index = Pos + 1;
        var valid = index >= 0 && index < _pattern.Length;

        return valid
            ? _pattern[index]
            : '\0';
    }

    public bool Equals(State other)
    {
        return 
            Current == other.Current && 
            InRun == other.InRun && 
            Pos == other.Pos && 
            RunIndex == other.RunIndex && 
            RunLength == other.RunLength;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Current, InRun, Pos, RunIndex, RunLength);
    }

    public static bool operator ==(State left, State right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(State left, State right)
    {
        return !left.Equals(right);
    }
}