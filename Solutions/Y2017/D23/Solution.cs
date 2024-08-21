using Solutions.Y2017.Common;

namespace Solutions.Y2017.D23;

[InputSpecificSolution]
[PuzzleInfo("Coprocessor Conflagration", Topics.Assembly|Topics.Assembly, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountExecutions(),
            2 => RunDisassembled(),
            _ => PuzzleNotSolvedString
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

    private static long RunDisassembled()
    {
        //  This method was written by disassembling the input. See the adjacent asm.txt for reference.
        //  The main function of the assembly program is counting composite numbers within a range,
        //  where the range boundaries are outputs of operations on specific constants in my input.
        //
        var count = 0L;
        for (var n = 106700L; n <= 123700L; n += 17)
        {
            if (!IsPrime(n))
            {
                count++;
            }
        }
        return count;
    }

    private static bool IsPrime(long n)
    {
        if (n % 2 == 0)
        {
            return false;
        }
        
        var boundary = (long)Math.Floor(Math.Sqrt(n));
        for (var i = 3L; i <= boundary; i += 2L)
        {
            if (n % i == 0)
            {
                return false;
            }   
        }

        return true;  
    }
}