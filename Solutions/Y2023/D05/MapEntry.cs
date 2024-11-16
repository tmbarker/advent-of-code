using Utilities.Numerics;

namespace Solutions.Y2023.D05;

public readonly record struct MapEntry(long DestStart, long SourceStart, long RangeLength)
{
    public long SourceMin => SourceStart;
    public long SourceMax => SourceStart + RangeLength - 1;
    public Range<long> SourceRange => new (SourceStart, SourceMax);

    public Range<long> Apply(Range<long> range)
    {
        return new Range<long>(
            min: DestStart + range.Min - SourceStart,
            max: DestStart + range.Max - SourceStart);
    }
    
    public static MapEntry Default(long min, long max)
    {
        return new MapEntry(
            DestStart: min,
            SourceStart: min,
            RangeLength: max - min + 1);
    }
}