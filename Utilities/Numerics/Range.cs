using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Numerics;

/// <summary>
///     A generic readonly integral interval value type.
/// </summary>
public readonly record struct Range<T> : IEnumerable<T> where T : INumber<T>
{
    public T Min { get; }
    public T Max { get; }
    public T Length { get; }

    public Range(T min, T max)
    {
        if (min > max)
        {
            throw new ArgumentException(
                $"{nameof(min)} must be less than or equal to {nameof(max)}", nameof(min));
        }
        
        Min = min;
        Max = max;
        Length = Max - Min + T.One;
    }
    
    /// <summary>
    ///     Query if two <see cref="Range{T}"/>'s overlap.
    /// </summary>
    /// <param name="a">The first <see cref="Range{T}"/></param>
    /// <param name="b">The second <see cref="Range{T}"/></param>
    /// <param name="overlap">The overlap between <paramref name="a"/> and <paramref name="b"/>, if it exists</param>
    /// <returns>A <see cref="bool"/> representing the success of the query</returns>
    public static bool Overlap(Range<T> a, Range<T> b, out Range<T> overlap)
    {
        if (a.Min > b.Max || b.Min > a.Max)
        {
            overlap = default;
            return false;
        }

        overlap = new Range<T>(
            min: T.Max(a.Min, b.Min),
            max: T.Min(a.Max, b.Max)); 
        return true;
    }

    /// <summary>
    ///     Construct a <see cref="Range{T}" /> from a single value.
    /// </summary>
    /// <param name="value">The single value contained in the returned <see cref="Range{T}" /></param>
    /// <returns>A <see cref="Range{T}" /> of length one, containing the specified <paramref name="value" /></returns>
    public static Range<T> Single(T value)
    {
        return new Range<T>(min: value, max: value);
    }

    /// <summary>
    ///     Parse a <see cref="Range{T}"/> from the provided <see cref="string"/>.
    /// </summary>
    /// <param name="s">The <see cref="string"/> to parse</param>
    /// <returns>The <see cref="Range{T}"/> represented by the provided <see cref="string"/></returns>
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

    /// <summary>
    ///     Query if the <see cref="Range{T}" /> inclusively contains the provided range
    /// </summary>
    /// <param name="a">The range to query</param>
    /// <returns>A <see cref="bool" /> representing the success of the query</returns>
    public bool Contains(Range<T> a)
    {
        return Contains(a.Min) && Contains(a.Max);
    }

    /// <summary>
    ///     Query if the <see cref="Range{T}" /> inclusively contains the provided value
    /// </summary>
    /// <param name="value">The value to query</param>
    /// <returns>A <see cref="bool" /> representing the success of the query</returns>
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