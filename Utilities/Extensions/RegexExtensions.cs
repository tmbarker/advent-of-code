using System.Text.RegularExpressions;

namespace Utilities.Extensions;

/// <summary>
/// A set of extension methods for selecting and parsing <see cref="Regex"/> object model instances
/// </summary>
public static class RegexExtensions
{
    /// <summary>
    /// Parse the value <see cref="string"/> of the <see cref="Match"/> as an <see cref="int"/>
    /// </summary>
    public static int ParseInt(this Match match)
    {
        return int.Parse(match.Value);
    }

    /// <summary>
    /// If the <see cref="Match"/> is successful assume that the specified <see cref="Group"/> was matched only once,
    /// and parse it as an <see cref="int"/>, otherwise return the <paramref name="default"/> value
    /// </summary>
    public static int ParseSingleIntOrDefault(this Match match, int group, int @default = 0)
    {
        return match.Success
            ? match.Groups[group].ParseSingleInt()
            : @default;
    }
    
    /// <summary>
    /// If the <see cref="Match"/> is successful assume that the specified <see cref="Group"/> was matched only once,
    /// and parse it as an <see cref="int"/>, otherwise return the <paramref name="default"/> value
    /// </summary>
    public static int ParseSingleIntOrDefault(this Match match, string group, int @default = 0)
    {
        return match.Success
            ? match.Groups[group].ParseSingleInt()
            : @default;
    }

    /// <summary>
    /// Parse the first <see cref="Capture"/> in the specified <see cref="Group"/> as an <see cref="int"/>
    /// </summary>
    public static int ParseSingleInt(this Group group)
    {
        return int.Parse(group.Value);
    }
    
    /// <summary>
    /// Parse all <see cref="Capture"/> instances in the specified <see cref="Group"/> as an <see cref="int"/>
    /// </summary>
    public static IEnumerable<int> ParseManyInt(this Group group)
    {
        return group.Captures.Select(capture => capture.ParseInt());
    }
    
    /// <summary>
    /// Parse the value <see cref="string"/> of the <see cref="Capture"/> as an <see cref="int"/>
    /// </summary>
    public static int ParseInt(this Capture capture)
    {
        return int.Parse(capture.Value);
    }

    /// <summary>
    /// Enumerate all <see cref="Capture"/> instances from the specified <see cref="Group"/> over the entire
    /// <see cref="MatchCollection"/>. This extension is effectively a Select Many wrapper.
    /// </summary>
    public static IEnumerable<string> SelectCaptures(this MatchCollection matches, int group)
    {
        return matches.SelectMany(match => match.Groups[group].Captures.Select(capture => capture.Value));
    }
}