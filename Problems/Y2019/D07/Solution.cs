using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D07;

/// <summary>
/// Amplification Circuit: https://adventofcode.com/2019/day/7
/// </summary>
public class Solution : SolutionBase2019
{
    public override int Day => 7;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => FindMaxSignalLinear(0, 4),
            1 => FindMaxSignalLooped(5, 9),
            _ => ProblemNotSolvedString,
        };
    }

    private int FindMaxSignalLinear(int minPhase, int maxPhase)
    {
        var max = 0;
        foreach (var permutation in GetPhasePermutations(minPhase, maxPhase))
        {
            var signal = 0;
            foreach (var phase in permutation)
            {
                var amp = IntCodeVm.Create(
                    program: LoadIntCodeProgram(),
                    inputs: new[] { phase, signal });
                
                amp.Run();
                signal = amp.OutputBuffer.Dequeue();
            }

            max = Math.Max(max, signal);
        }
        
        return max;
    }

    private int FindMaxSignalLooped(int minPhase, int maxPhase)
    {
        var max = 0;
        var amps = new Queue<IntCodeVm>();
        
        foreach (var permutation in GetPhasePermutations(minPhase, maxPhase))
        {
            var signal = 0;
            foreach (var phase in permutation)
            {
                amps.Enqueue(IntCodeVm.Create(
                    program: LoadIntCodeProgram(),
                    input: phase));
            }

            while (amps.Any())
            {
                var amp = amps.Dequeue();
                amp.InputBuffer.Enqueue(signal);

                var ec = amp.Run();
                signal = amp.OutputBuffer.Dequeue();

                if (ec != IntCodeVm.ExitCode.Halted)
                {
                    amps.Enqueue(amp);
                }
            }

            max = Math.Max(max, signal);
        }
        
        return max;
    }

    private static IEnumerable<IList<int>> GetPhasePermutations(int minPhase, int maxPhase)
    {
        var permutations = new List<IList<int>>();
        var phases = new List<int>(Enumerable.Range(minPhase, maxPhase - minPhase + 1));

        return PermuteMembers(phases, 0, phases.Count - 1, permutations);
    }

    private static IList<IList<int>> PermuteMembers(IList<int> members, int start, int end, IList<IList<int>> permutations)
    {
        if (start == end)
        {
            permutations.Add(new List<int>(members));
        }
        else
        {
            for (var i = start; i <= end; i++)
            {
                SwapMembers(members, start, i);
                PermuteMembers(members, start + 1, end, permutations);
                SwapMembers(members, start, i);
            }
        }

        return permutations;
    }
    
    private static void SwapMembers(IList<int> members, int idx1, int idx2)
    {
        if (idx1 != idx2)
        {
            (members[idx1], members[idx2]) = (members[idx2], members[idx1]);
        }
    }
}