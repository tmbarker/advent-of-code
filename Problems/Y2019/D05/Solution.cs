using Problems.Y2019.IntCode;

namespace Problems.Y2019.D05;

[PuzzleInfo("Sunny with a Chance of Asteroids", Topics.IntCode, Difficulty.Easy)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        var program = LoadIntCodeProgram();
        return part switch
        {
            1 => RunTestProgram(program, systemId: 1),
            2 => RunTestProgram(program, systemId: 5),
            _ => ProblemNotSolvedString
        };
    }

    private static long RunTestProgram(IList<long> program, int systemId)
    {
        var vm = IntCodeVm.Create(program, systemId);
        vm.Run();
        return vm.OutputBuffer.Last();
    }
}