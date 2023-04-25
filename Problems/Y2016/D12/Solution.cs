using Problems.Common;

namespace Problems.Y2016.D12;

/// <summary>
/// Leonardo's Monorail: https://adventofcode.com/2016/day/12
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var program = GetInputLines();
        var tokens = program.Select(line => line.Split(' ')).ToList();

        return part switch
        {
            1 => RunProgram(tokens, c: 0),
            2 => RunProgram(tokens, c: 1),
            _ => ProblemNotSolvedString
        };
    }

    private static long RunProgram(IList<string[]> program, long c)
    {
        var vm = new Vm { ["c"] = c };
        vm.Run(program);
        return vm["a"];
    }
}