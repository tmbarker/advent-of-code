using Problems.Common;

namespace Problems.Y2016.D25;

/// <summary>
/// Clock Signal: https://adventofcode.com/2016/day/25
/// </summary>
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            1 => GetLowestSatisfactoryInput(),
            _ => ProblemNotSolvedString
        };
    }

    private long GetLowestSatisfactoryInput()
    {
        var input = GetInputLines();
        var tokens = input.Select(line => line.Split(' ')).ToList();

        //  The below code was created after analyzing/disassembling the input. The program
        //  multiplies two numbers, and adds the input (value in register "a") to the product:
        //
        //  n = input + c1 * c2;
        //
        //  It then calculates the binary representation of the number by repeatedly dividing
        //  by 2. Each bit is printed. Once the number is reduced to 0, it is reset, and the bits
        //  are computed again.
        //
        //  In order to infinitely print alternating 0s and 1s, the number must have a binary
        //  representation of the form 0101010... Then, we simply find the smallest number 
        //  larger than c1 * c2, and return the difference.
        //
        var c1 = long.Parse(tokens[1][1]);
        var c2 = long.Parse(tokens[2][1]);
        var c3 = c1 * c2;
        var total = 1;

        while (total < c3)
        {
            if (total % 2 == 0)
            {
                total = 2 * total + 1;
            }
            else
            {
                total *= 2;
            }
        }

        return total - c3;
    }
}