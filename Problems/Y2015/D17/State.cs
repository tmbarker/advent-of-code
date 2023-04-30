using Utilities.Extensions;

namespace Problems.Y2015.D17;

public readonly struct State : IEquatable<State>
{
    private readonly string _key;
    
    public HashSet<Cup> Unused { get; }
    public int NumUsed { get; }
    public int TotalVolume { get; }
    
    public State(HashSet<Cup> unused, int numUsed, int totalVolume)
    {
        _key = string.Join(',', unused.OrderBy(c => c.Id));
        Unused = unused;
        NumUsed = numUsed;
        TotalVolume = totalVolume;
    }

    public State AfterUsing(Cup cup)
    {
        return new State(
            unused: Unused.Except(cup).ToHashSet(),
            numUsed: NumUsed + 1,
            totalVolume: TotalVolume + cup.Size);
    }
    
    public bool Equals(State other)
    {
        return _key == other._key;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _key.GetHashCode();
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