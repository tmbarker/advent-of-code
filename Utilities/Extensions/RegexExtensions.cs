using System.Text.RegularExpressions;

namespace Utilities.Extensions;

public static class RegexExtensions
{
    public static int ParseInt(this Match match)
    {
        return int.Parse(match.Value);
    }

    public static int ParseInt(this Group group)
    {
        return int.Parse(group.Value);
    }
    
    public static int ParseInt(this Capture capture)
    {
        return int.Parse(capture.Value);
    }

    public static IEnumerable<string> SelectCaptures(this Match match, string group)
    {
        return match.Groups[group].Captures.Select(c => c.Value);
    }
    
    public static IEnumerable<string> SelectCaptures(this Match match, int group)
    {
        return match.Groups[group].Captures.Select(c => c.Value);
    }
    
    public static IEnumerable<string> SelectCaptures(this MatchCollection matches, string group)
    {
        return matches.SelectMany(match => match.Groups[group].Captures.Select(capture => capture.Value));
    }

    public static IEnumerable<string> SelectCaptures(this MatchCollection matches, int group)
    {
        return matches.SelectMany(match => match.Groups[group].Captures.Select(capture => capture.Value));
    }
}