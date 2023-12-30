using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Numerics;

/// <summary>
/// A generic readonly interval value type
/// </summary>
public readonly struct Range<T>(T min, T max) : IEnumerable<T>, IEquatable<Range<T>>
    where T : IBinaryNumber<T>
{
    public T Min { get; } = min;
    public T Max { get; } = max;
    public T Length => Max - Min + T.One;
    
    public static bool Overlap(Range<T> a, Range<T> b, out  Range<T> overlap)
    {
        var hasOverlap = a.Max >= b.Min && a.Min <= b.Max;
        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }

        var limits = new[] { a.Min, a.Max, b.Min, b.Max }.Order().ToList();
        overlap = new Range<T>(
            min: limits[1],
            max: limits[2]);
        return true;
    }

    public bool Contains(Range<T> a)
    {
        return Contains(a.Min) && Contains(a.Max);
    }
    
    public bool Contains(T value)
    {
        return value >= Min && value <= Max;
    }

    public override string ToString()
    {
        return $"[{Min}..{Max}]";
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var v = Min; v <= Max; v++)
        {
            yield return v;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(Range<T> other)
    {
        return Min == other.Min && Max == other.Max;
    }

    public override bool Equals(object? obj)
    {
        return obj is Range<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }

    public static bool operator ==(Range<T> left, Range<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Range<T> left, Range<T> right)
    {
        return !left.Equals(right);
    }

    public static Range<T> Single(T value)
    {
        return new Range<T>(min: value, max: value);
    }
    
    public static Range<T> Parse(string s)
    {
        //  Ranges are often written as "<min>-<max>", we can use a regex to attempt to match this pattern first
        //  in order to avoid issues with parsing the range delimiter '-' as a negative sign.
        //
        var match = Regex.Match(input: s, pattern: @"(?<Min>-?\d+)\s*-\s*(?<Max>-?\d+)", RegexOptions.Compiled);
        if (match.Success)
        {
            return new Range<T>(
                min: match.Groups["Min"].Value.ParseNumber<T>(),
                max: match.Groups["Max"].Value.ParseNumber<T>());
        }
        
        var numbers = s.ParseNumbers<T>();
        return new Range<T>(min: numbers[0], max: numbers[1]);
    }
}