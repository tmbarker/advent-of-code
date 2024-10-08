using System.Text.RegularExpressions;

namespace Solutions.Y2015.D08;

[PuzzleInfo("Matchsticks", Topics.RegularExpressions, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var strings = GetInputLines();
        return part switch
        {
            1 => strings.Sum(raw => raw.Length - ToInMemory(raw).Length),
            2 => strings.Sum(raw => ToEscaped(raw).Length - raw.Length),
            _ => PuzzleNotSolvedString
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