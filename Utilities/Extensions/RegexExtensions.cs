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
}