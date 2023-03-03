using System.Text.RegularExpressions;

namespace Utilities.Extensions;

public static class StringExtensions
{
    private static readonly Regex NumberRegex = new(@"(-?\d+)");
    
    public static IList<int> Numbers(this string str)
    {
        var matches = NumberRegex.Matches(str);
        var numbers = new List<int>(matches.Select(m => int.Parse(m.Value)));

        return numbers;
    }
}