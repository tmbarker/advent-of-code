using Problems.Common;

namespace Problems.Y2015.D23;

/// <summary>
/// Opening the Turing Lock: https://adventofcode.com/2015/day/23
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Emulate(a: 0L),
            2 => Emulate(a: 1L),
            _ => ProblemNotSolvedString
        };
    }

    private long Emulate(long a)
    {
        var program = GetInputLines();
        var vm = new Vm { ['a'] = a };

        vm.Run(program);
        return vm['b'];
    }
}