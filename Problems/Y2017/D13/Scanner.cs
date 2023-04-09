namespace Problems.Y2017.D13;

public readonly struct Scanner
{
    public int Depth { get; }
    public int Range { get; }

    public int Severity => Depth * Range;

    public Scanner(int depth, int range)
    {
        Depth = depth;
        Range = range;
    }
}