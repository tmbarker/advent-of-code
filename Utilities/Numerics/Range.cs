using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Numerics;

/// <summary>
///     A generic readonly interval value type
/// </summary>
public readonly record struct Range<T>(T Min, T Max) : IEnumerable<T> where T : IBinaryNumber<T>
{
    public T Length => Max - Min + T.One;

    public static bool Overlap(Range<T> a, Range<T> b, out Range<T> overlap)
    {
        if (a.Min > b.Max || b.Min > a.Max)
        {
            overlap = default;
            return false;
        }

        overlap = new Range<T>(
            Min: T.Max(a.Min, b.Min),
            Max: T.Min(a.Max, b.Max));
        return true;
    }

    public static Range<T> Single(T value)
    {
        return new Range<T>(Min: value, Max: value);
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
                Min: match.Groups["Min"].Value.ParseNumber<T>(),
                Max: match.Groups["Max"].Value.ParseNumber<T>());
        }

        var numbers = s.ParseNumbers<T>();
        return new Range<T>(Min: numbers[0], Max: numbers[1]);
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
}