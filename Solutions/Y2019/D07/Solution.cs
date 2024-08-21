using Solutions.Y2019.IntCode;
using Utilities.Extensions;

namespace Solutions.Y2019.D07;

[PuzzleInfo("Amplification Circuit", Topics.IntCode|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => FindMaxSignalLinear(minPhase: 0, maxPhase: 4),
            2 => FindMaxSignalLooped(minPhase: 5, maxPhase: 9),
            _ => PuzzleNotSolvedString
        };
    }

    private long FindMaxSignalLinear(int minPhase, int maxPhase)
    {
        var max = 0L;
        var program = LoadIntCodeProgram();
        
        foreach (var permutation in GetPhasePermutations(minPhase, maxPhase))
        {
            var signal = 0L;
            foreach (var phase in permutation)
            {
                var amp = IntCodeVm.Create(
                    program: program,
                    inputs: [phase, signal]);
                
                amp.Run();
                signal = amp.OutputBuffer.Dequeue();
            }

            max = Math.Max(max, signal);
        }
        
        return max;
    }

    private long FindMaxSignalLooped(int minPhase, int maxPhase)
    {
        var max = 0L;
        var amps = new Queue<IntCodeVm>();
        
        foreach (var permutation in GetPhasePermutations(minPhase, maxPhase))
        {
            var signal = 0L;
            var program = LoadIntCodeProgram();
            
            foreach (var phase in permutation)
            {
                amps.Enqueue(IntCodeVm.Create(
                    program: program,
                    input: phase));
            }

            while (amps.Count > 0)
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

    private static IEnumerable<IEnumerable<int>> GetPhasePermutations(int minPhase, int maxPhase)
    {
        return Enumerable.Range(minPhase, maxPhase - minPhase + 1).Permute();
    }
}