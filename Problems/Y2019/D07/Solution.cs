using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D07;

/// <summary>
/// Amplification Circuit: https://adventofcode.com/2019/day/7
/// </summary>
public class Solution : SolutionBase2019
{
    private const int Stages = 5;
    
    public override int Day => 7;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => FindMaxSignal(0, 4),
            _ => ProblemNotSolvedString,
        };
    }

    private int FindMaxSignal(int minPhase, int maxPhase)
    {
        var max = 0;
        var amplifierCircuit = new AmplifierCircuit();
        var vm = new Vm
        {
            InputSource = amplifierCircuit,
            OutputSink = amplifierCircuit,
        };

        foreach (var permutation in GetPhasePermutations(minPhase, maxPhase))
        {
            amplifierCircuit.Reset(permutation);
            for (var stage = 0; stage < Stages; stage++)
            {
                vm.Run(LoadIntCodeProgram());
            }

            max = Math.Max(max, amplifierCircuit.Signal);
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