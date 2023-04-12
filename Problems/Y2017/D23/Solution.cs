using Problems.Y2017.Common;

namespace Problems.Y2017.D23;

/// <summary>
/// Coprocessor Conflagration: https://adventofcode.com/2017/day/23
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 23;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountExecutions(),
            _ => ProblemNotSolvedString
        };
    }
    
    private long CountExecutions()
    {
        var count = 0L;
        var vm = new Vm(program: GetInputLines());

        void OnMulExecuted()
        {
            count++;
        }

        vm.RegisterListener(op:"mul", listener: OnMulExecuted);
        vm.Run(token: default);

        return count;
    }
}