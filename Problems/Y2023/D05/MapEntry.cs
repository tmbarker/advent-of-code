namespace Problems.Y2023.D05;

public readonly struct MapEntry
{
    private long SourceStart { get; }
    private long DestStart { get; }
    private long RangeLength { get; }

    public long SourceMin => SourceStart;
    public long SourceMax => SourceStart + RangeLength - 1;
    public Range SourceRange => new (SourceStart, SourceMax);

    public MapEntry(long destStart, long sourceStart, long rangeLength)
    {
        DestStart = destStart;
        SourceStart = sourceStart;
        RangeLength = rangeLength;
    }

    public Range Apply(Range range)
    {
        return new Range(
            min: DestStart + range.Min - SourceStart,
            max: DestStart + range.Max - SourceStart);
    }
    
    public static MapEntry Default(long min, long max)
    {
        return new MapEntry(
            destStart: min,
            sourceStart: min,
            rangeLength: max - min + 1);
    }
}