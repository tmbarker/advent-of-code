namespace Solutions.Y2017.D05;

[PuzzleInfo("A Maze of Twisty Trampolines, All Alike", Topics.Simulation, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var offsets = ParseInputLines(parseFunc: int.Parse);
        return part switch
        {
            1 => CountSteps(offsets, offsetModifier: offset => offset + 1),
            2 => CountSteps(offsets, offsetModifier: offset => offset >= 3 ? offset - 1 : offset + 1),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountSteps(int[] offsets, Func<int, int> offsetModifier)
    {
        var ip = 0;
        var steps = 0;
        
        while (ip >= 0 && ip < offsets.Length)
        {
            var jumpTo = ip + offsets[ip];
            offsets[ip] = offsetModifier(offsets[ip]);
            steps++;
            ip = jumpTo;
        }

        return steps;
    }
}