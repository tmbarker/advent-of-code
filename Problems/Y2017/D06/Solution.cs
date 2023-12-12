using Utilities.Extensions;

namespace Problems.Y2017.D06;

[PuzzleInfo("Memory Reallocation", Topics.BitwiseOperations, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var state = ParseState(input);
        
        return part switch
        {
            1 => GetCycle(state),
            2 => GetLoopLength(state),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetCycle(MemoryBankState state)
    {
        var seen = new HashSet<MemoryBankState>();
        while (seen.Add(state))
        {
            state = state.Reallocate();
        }

        return seen.Count;
    }

    private static int GetLoopLength(MemoryBankState state)
    {
        var seen = new HashSet<MemoryBankState>();
        while (seen.Add(state))
        {
            state = state.Reallocate();
        }

        seen.Clear();
        while (seen.Add(state))
        {
            state = state.Reallocate();
        }

        return seen.Count;
    }

    private static MemoryBankState ParseState(string input)
    {
        var ulongs = input
            .ParseLongs()
            .Select(l => (ulong)l)
            .ToList();

        return new MemoryBankState(banks: ulongs);
    }
}