using System.Text.RegularExpressions;

namespace Utilities.Extensions;

public static class StringExtensions
{
    private static readonly Regex NumberRegex = new(@"(-?\d+)");
    private static readonly Regex WhitespaceRegex = new(@"\s+");

    public static IList<int> ParseInts(this string str)
    {
        var matches = NumberRegex.Matches(str);
        var numbers = new List<int>(matches.Select(m => int.Parse(m.Value)));

        return numbers;
    }

    public static int ParseInt(this string str)
    {
        return ParseInts(str)[0];
    }

    public static IList<long> ParseLongs(this string str)
    {
        var matches = NumberRegex.Matches(str);
        var numbers = new List<long>(matches.Select(m => long.Parse(m.Value)));

        return numbers;
    }

    public static string TrimWhitespace(this string str)
    {
        return WhitespaceRegex.Replace(input: str, replacement: string.Empty);
    }
}