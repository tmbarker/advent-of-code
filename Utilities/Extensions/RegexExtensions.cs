using System.Text.RegularExpressions;

namespace Utilities.Extensions;

/// <summary>
///     A set of extension methods for selecting and parsing <see cref="Regex" /> object model instances
/// </summary>
public static class RegexExtensions
{
    /// <summary>
    ///     If the <see cref="Match" /> is successful parse it as an <see cref="int" />, otherwise return
    ///     the <paramref name="default" /> value
    /// </summary>
    public static int ParseIntOrDefault(this Match match, int group = 1, int @default = 0)
    {
        return match.Success
            ? match.Groups[group].ParseInt()
            : @default;
    }

    /// <summary>
    ///     Parse all <see cref="Capture" /> instances in the specified <see cref="Group" /> as an <see cref="int" />
    /// </summary>
    public static IEnumerable<int> ParseInts(this Group group)
    {
        return group.Captures.Select(capture => capture.ParseInt());
    }

    /// <summary>
    ///     Parse the value <see cref="string" /> of the <see cref="Capture" /> as an <see cref="int" />
    /// </summary>
    public static int ParseInt(this Capture capture)
    {
        return int.Parse(capture.Value);
    }

    /// <summary>
    ///     Parse the value <see cref="string" /> of the <see cref="Capture" /> as a <see cref="long" />
    /// </summary>
    public static long ParseLong(this Capture capture)
    {
        return long.Parse(capture.Value);
    }

    /// <summary>
    ///     Enumerate all <see cref="Match" /> instances from the <see cref="MatchCollection" />.
    /// </summary>
    public static IList<string> SelectValues(this MatchCollection matches)
    {
        return matches
            .Select(match => match.Value)
            .ToList();
    }

    /// <summary>
    ///     Enumerate all <see cref="Capture" /> instances from the specified <see cref="Group" /> over the entire
    ///     <see cref="MatchCollection" />. This extension is effectively a Select Many wrapper.
    /// </summary>
    public static IList<string> SelectCaptures(this MatchCollection matches, int group)
    {
        return matches
            .SelectMany(match => match.Groups[group].Captures.Select(capture => capture.Value))
            .ToList();
    }

    /// <summary>
    ///     Enumerate all <see cref="Capture" /> instances from the specified <see cref="Group" /> over the entire
    ///     <see cref="MatchCollection" />. This extension is effectively a Select Many wrapper.
    /// </summary>
    public static IList<string> SelectCaptures(this MatchCollection matches, string group)
    {
        return matches
            .SelectMany(match => match.Groups[group].Captures.Select(capture => capture.Value))
            .ToList();
    }
}