using Problems.Attributes;
using Problems.Common;
using Problems.Y2016.Common;

namespace Problems.Y2016.D23;

/// <summary>
/// Safe Cracking: https://adventofcode.com/2016/day/23
/// </summary>
[InputSpecific]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var tokens = input.Select(line => line.Split(' ')).ToList();

        return part switch
        {
            1 => RunProgram(tokens, numEggs: 7L),
            2 => RunProgramOptimized(tokens, numEggs: 12L),
            _ => ProblemNotSolvedString
        };
    }
    
    private static long RunProgram(IList<string[]> program, long numEggs)
    {
        var vm = new Vm { ["a"] = numEggs };
        vm.Run(program);
        return vm["a"];
    }
    
    private static long RunProgramOptimized(IList<string[]> program, long numEggs)
    {
        //  This calculation was determined by analyzing the assembly. The program
        //  (after all toggles have been executed) returns the sum of the factorial
        //  of the value provided to register "a", and the products of the first
        //  args to the "cpy" and "jnz" instructions on lines 19 and 20. The program
        //  is extremely slow because it calculates the factorial by incrementing by one
        //
        var c1 = long.Parse(program[19][1]);
        var c2 = long.Parse(program[20][1]);

        return c1 * c2 + Factorial(numEggs);
    }

    private static long Factorial(long n)
    {
        var product = 1L;
        while (n > 0)
        {
            product *= n--;
        }
        return product;
    }
}