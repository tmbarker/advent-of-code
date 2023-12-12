using Utilities.Extensions;

namespace Problems.Y2016.D04;

[PuzzleInfo("Security Through Obscurity", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => SumValid(),
            2 => Decrypt(),
            _ => ProblemNotSolvedString
        };
    }

    private int SumValid()
    {
        var lines = GetInputLines();
        var sum = 0;

        foreach (var line in lines)
        {
            var counts = new Dictionary<char, int>();
            var sectorId = Math.Abs(line.ParseInt());
            var checksum = line[^6..^1];
        
            for (var i = 0; i < line.Length && line[i] != '['; i++)
            {
                if (!char.IsLetter(line[i]))
                {
                    continue;
                }
            
                counts.TryAdd(line[i], 0);
                counts[line[i]]++;
            }

            var ordered = counts.OrderByDescending(kvp => 26 * counts[kvp.Key] + 'z' - kvp.Key);
            var concat = string.Concat(ordered.Take(5).Select(kvp => kvp.Key));

            if (concat == checksum)
            {
                sum += sectorId;
            }
        }

        return sum;
    }

    private int Decrypt()
    {
        foreach (var line in GetInputLines())
        {
            var sectorId = Math.Abs(line.ParseInt());
            var decrypt = line.SkipLast(7).ToArray();
            
            for (var i = 0; i < decrypt.Length; i++)
            {
                switch (decrypt[i])
                {
                    case '-':
                        decrypt[i] = ' ';
                        continue;
                    case {} when char.IsNumber(decrypt[i]):
                        continue;
                    default:
                        var inRangeInt = decrypt[i] - 'a';
                        var rotated = (inRangeInt + sectorId) % 26;
                        decrypt[i] = (char)('a' + rotated);
                        continue;
                }
            }

            if (string.Concat(decrypt).Contains("northpole"))
            {
                return sectorId;
            }
        }

        throw new NoSolutionException();
    }
}