using Problems.Common;
using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D09;

/// <summary>
/// Sensor Boost: https://adventofcode.com/2019/day/9
/// </summary>
public class Solution : SolutionBase2019
{
    public override int Day => 9;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 =>  RunBoostProgram(input: 1L),
            2 =>  RunBoostProgram(input: 2L),
            _ => ProblemNotSolvedString
        };
    }

    private long RunBoostProgram(long input)
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram(), input);
        var ec = vm.Run();
        
        return ec == IntCodeVm.ExitCode.Halted
            ? vm.OutputBuffer.Dequeue()
            : throw new NoSolutionException();
    }
}