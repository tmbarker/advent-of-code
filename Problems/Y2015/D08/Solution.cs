using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;

namespace Problems.Y2015.D08;

/// <summary>
/// Matchsticks: https://adventofcode.com/2015/day/8
/// </summary>
[Favourite("Matchsticks", Topics.RegularExpressions, Difficulty.Medium)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var strings = GetInputLines();
        return part switch
        {
            1 => strings.Sum(raw => raw.Length - ToInMemory(raw).Length),
            2 => strings.Sum(raw => ToEscaped(raw).Length - raw.Length),
            _ => ProblemNotSolvedString
        };
    }
    
    private static string ToInMemory(string str)
    {
        str = Regex.Replace(str, pattern: @"^""", replacement: "");
        str = Regex.Replace(str, pattern: @"""$", replacement: "");
        str = Regex.Replace(str, pattern: @"\\\\", replacement: "\\");
        str = Regex.Replace(str, pattern: @"\\""", replacement: "\"");
        str = Regex.Replace(str, pattern: @"\\x[a-f0-9][a-f0-9]", replacement: "U");
        
        return str;
    }

    private static string ToEscaped(string str)
    {
        str = Regex.Replace(str, pattern: @"\\\\(?!\"")", replacement: "\\\\\\\\");
        str = Regex.Replace(str, pattern: @"\\""", replacement: "\\\\\\\"");
        str = Regex.Replace(str, pattern: @"\\x[a-f0-9][a-f0-9]", replacement: "\\\\xUU");
        str = Regex.Replace(str, pattern: @"^""", replacement: "\"\\\"");
        str = Regex.Replace(str, pattern: @"""$", replacement: "\"\\\"");
        
        return str;
    }
}