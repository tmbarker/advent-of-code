using System.Text.RegularExpressions;

namespace Problems.Y2016.D14;

[PuzzleInfo("One-Time Pad", Topics.Hashing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Regex CandidateRegex = new(@"(?<C>[a-z0-9])\1\1");
    
    public override object Run(int part)
    {
        var salt = GetInputText();
        return part switch
        {
            1 => FindKeys(salt, stretches: 0000, count: 64),
            2 => FindKeys(salt, stretches: 2016, count: 64),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindKeys(string salt, int stretches, int count)
    {
        var indices = new List<int>();
        var hashes = new HashSequence(salt, stretches, count: 1000);

        for (var i = 0; indices.Count < count; i++)
        {
            var candidate = hashes[i];
            var match = CandidateRegex.Match(candidate);

            if (!match.Success)
            {
                continue;
            }

            var repeat = match.Groups["C"].Value.Single();
            var need = new string(repeat, 5);
            
            for (var j = 1; j <= 1000; j++)
            {
                if (hashes[i + j].Contains(need))
                {
                    indices.Add(i);
                    break;
                }
            }
        }
        
        return indices.Last();
    }
}