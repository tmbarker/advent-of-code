using Problems.Common;

namespace Problems.Y2017.D04;

using Passphrase = IList<string>;

/// <summary>
/// High-Entropy Passphrases: https://adventofcode.com/2017/day/4
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var passphrases = ParseInputLines(parseFunc: ParsePassphrase);
        return part switch
        {
            1 => CountValidDistinctWords(passphrases),
            2 => CountValidNoAnagrams(passphrases),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountValidDistinctWords(IEnumerable<Passphrase> passphrases)
    {
        return passphrases.Count(passphrase => new HashSet<string>(passphrase).Count == passphrase.Count);
    }

    private static int CountValidNoAnagrams(IEnumerable<Passphrase> passphrases)
    {
        return passphrases.Count(passphrase =>
        {
            var sortedWords = passphrase.Select(word => string.Concat(word.Order()));
            var sortedSet = new HashSet<string>(sortedWords);

            return sortedSet.Count == passphrase.Count;
        });
    }

    private static Passphrase ParsePassphrase(string line)
    {
        return line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
}