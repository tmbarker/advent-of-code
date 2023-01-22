using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D05;

/// <summary>
/// Sunny with a Chance of Asteroids: https://adventofcode.com/2019/day/5
/// </summary>
public class Solution : SolutionBase2019
{
    private const int SystemId1 = 1;
    private const int SystemId2 = 5;
    
    public override int Day => 5;
    
    public override object Run(int part)
    {
        var program = LoadIntCodeProgram();
        return part switch
        {
            0 => RunTestProgram(program, SystemId1),
            1 => RunTestProgram(program, SystemId2),
            _ => ProblemNotSolvedString,
        };
    }

    private static long RunTestProgram(IList<long> program, int systemId)
    {
        var vm = IntCodeVm.Create(program);
        vm.InputBuffer.Enqueue(systemId);
        vm.Run();
        return vm.OutputBuffer.Last();
    }
}