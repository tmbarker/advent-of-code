namespace Problems.Y2023.D05;

public readonly struct Range
{
    public long Min { get; }
    public long Max { get; }

    public Range(long min, long max)
    {
        Min = min;
        Max = max;
    }

    public static Range Single(long value)
    {
        return new Range(min: value, max: value);
    }
    
    public static bool Contains(Range a, Range contains)
    {
        return a.Min <= contains.Min && a.Max >= contains.Max;
    }
    
    public static bool Overlaps(Range a, Range b, out  Range overlap)
    {
        var check = a.Max >= b.Min && a.Min <= b.Max;
        var limits = new[] { a.Min, a.Max, b.Min, b.Max }.Order().ToList();

        overlap = check
            ? new Range(min: limits[1], max: limits[2])
            : default;
        
        return check;
    }
}