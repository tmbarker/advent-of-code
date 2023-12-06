using Problems.Common;

namespace Problems.Y2017.D05;

/// <summary>
/// A Maze of Twisty Trampolines, All Alike: https://adventofcode.com/2017/day/5
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var offsets = ParseInputLines(parseFunc: int.Parse).ToList();
        return part switch
        {
            1 => CountSteps(offsets, offsetModifier: offset => offset + 1),
            2 => CountSteps(offsets, offsetModifier: offset => offset >= 3 ? offset - 1 : offset + 1),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountSteps(IList<int> offsets, Func<int, int> offsetModifier)
    {
        var ip = 0;
        var steps = 0;
        
        while (ip >= 0 && ip < offsets.Count)
        {
            var jumpTo = ip + offsets[ip];
            offsets[ip] = offsetModifier(offsets[ip]);
            steps++;
            ip = jumpTo;
        }

        return steps;
    }
}