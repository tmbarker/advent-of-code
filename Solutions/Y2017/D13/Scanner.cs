namespace Solutions.Y2017.D13;

public readonly struct Scanner(int depth, int range)
{
    public int Depth { get; } = depth;
    public int Range { get; } = range;

    public int Severity => Depth * Range;
}