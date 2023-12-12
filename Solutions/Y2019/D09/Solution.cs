using Solutions.Y2019.IntCode;

namespace Solutions.Y2019.D09;

[PuzzleInfo("Sensor Boost", Topics.IntCode, Difficulty.Easy)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => RunBoostProgram(input: 1L),
            2 => RunBoostProgram(input: 2L),
            _ => ProblemNotSolvedString
        };
    }

    private long RunBoostProgram(long input)
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram(), input);
        var ec = vm.Run();
        
        return ec == IntCodeVm.ExitCode.Halted
            ? vm.OutputBuffer.Dequeue()
            : throw new NoSolutionException(message: $"Invalid VM exit code [{ec}]");
    }
}