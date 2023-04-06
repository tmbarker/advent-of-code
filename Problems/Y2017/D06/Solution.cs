using Problems.Attributes;
using Problems.Y2017.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D06;

/// <summary>
/// Memory Reallocation: https://adventofcode.com/2017/day/6
/// </summary>
[Favourite("Memory Reallocation", Topics.BitwiseOperations, Difficulty.Easy)]
public class Solution : SolutionBase2017
{
    public override int Day => 6;
    
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