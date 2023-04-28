using Problems.Common;
using Utilities.Hashing;

namespace Problems.Y2015.D04;

/// <summary>
/// The Ideal Stocking Stuffer: https://adventofcode.com/2015/day/4
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var key = GetInputText();
        return part switch
        {
            1 => FindHashSeed(key, zeroes: 5),
            2 => FindHashSeed(key, zeroes: 6),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindHashSeed(string key, int zeroes)
    {
        var provider = new Md5Provider();
        var hash = string.Empty;
        var target = new string(c: '0', zeroes);
        var value = 0;

        while (!hash.StartsWith(target))
        {
            hash = provider.GetHashHex($"{key}{++value}");
        }

        return value;
    }
}