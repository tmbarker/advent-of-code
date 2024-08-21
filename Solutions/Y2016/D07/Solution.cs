using System.Text.RegularExpressions;

namespace Solutions.Y2016.D07;

[PuzzleInfo("Internet Protocol Version 7", Topics.RegularExpressions, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Regex AbaRegex =      new(pattern: @"([a-z])(?!\1)([a-z])\1");
    private static readonly Regex AbbaRegex =     new(pattern: @"([a-z])(?!\1)([a-z])\2\1");
    private static readonly Regex SupernetRegex = new(pattern: @"([a-z]+)(?![^\[]*\])");
    private static readonly Regex HypernetRegex = new(pattern: @"\[([a-z]+)\]");
    
    public override object Run(int part)
    {
        var ipAddresses = GetInputLines();
        return part switch
        {
            1 => ipAddresses.Count(SupportsTls),
            2 => ipAddresses.Count(SupportsSsl),
            _ => PuzzleNotSolvedString
        };
    }

    private static bool SupportsTls(string address)
    {
        var sn = SupernetRegex.Matches(address).Select(m => m.Value);
        var hn = HypernetRegex.Matches(address).Select(m => m.Value);

        return
            sn.Any(s =>  AbbaRegex.IsMatch(s)) &&
            hn.All(s => !AbbaRegex.IsMatch(s));
    }
    
    private static bool SupportsSsl(string address)
    {
        var sn = SupernetRegex.Matches(address).Select(m => m.Value);
        var hn = HypernetRegex.Matches(address).Select(m => m.Value);

        var snAbas = sn.SelectMany(CollectAbas).ToHashSet();
        var hnAbas = hn.SelectMany(CollectAbas).ToHashSet();

        return snAbas.Any(snAba => hnAbas.Any(hnAba => hnAba[0] == snAba[1] && hnAba[1] == snAba[0]));
    }

    private static IEnumerable<string> CollectAbas(string seq)
    {
        for (var i = 0; i <= seq.Length - 3; i++)
        {
            var match = AbaRegex.Match(seq, beginning: i, length: 3);
            if (match.Success)
            {
                yield return match.Value;
            }
        }
    }
}