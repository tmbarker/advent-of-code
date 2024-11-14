using Utilities.Hashing;

namespace Solutions.Y2015.D04;

[PuzzleInfo("The Ideal Stocking Stuffer", Topics.Hashing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var key = GetInputText();
        return part switch
        {
            1 => FindHashSeed(key, zeroes: 5),
            2 => FindHashSeed(key, zeroes: 6),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindHashSeed(string key, int zeroes)
    {
        using var provider = new Md5Provider();
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