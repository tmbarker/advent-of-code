using Utilities.Extensions;

namespace Solutions.Y2015.D10;

[PuzzleInfo("Elves Look, Elves Say", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var sequence = input
            .Select(StringExtensions.AsDigit)
            .ToList();
        
        return part switch
        {
            1 => LookAndSay(sequence, rounds: 40),
            2 => LookAndSay(sequence, rounds: 50),
            _ => ProblemNotSolvedString
        };
    }

    private static int LookAndSay(IList<int> sequence, int rounds)
    {
        for (var i = 0; i < rounds; i++)
        {
            sequence = LookAndSay(sequence);
        }

        return sequence.Count;
    }

    private static IList<int> LookAndSay(IList<int> sequence)
    {
        var next = new List<int>();
        var prev = sequence[0];
        var count = 1;

        for (var i = 1; i < sequence.Count; i++)
        {
            if (sequence[i] == prev)
            {
                count++;
                continue;
            }
            
            next.Add(count);
            next.Add(prev);
            prev = sequence[i];
            count = 1;
        }
        
        next.Add(count);
        next.Add(prev);

        return next;
    }
}