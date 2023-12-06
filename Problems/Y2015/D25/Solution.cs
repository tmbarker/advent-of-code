using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2015.D25;

/// <summary>
/// Let It Snow: https://adventofcode.com/2015/day/25
/// </summary>
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            1 => GetCode(),
            _ => ProblemNotSolvedString
        };
    }

    private ulong GetCode()
    {
        var input = GetInputText();
        var numbers = input.ParseInts();
        var tx = (ulong)numbers[1];
        var ty = (ulong)numbers[0];
        
        var x = 1UL;
        var y = 1UL;
        var code = 20151125UL;

        while (true)
        {
            x += 1;
            y -= 1;

            if (y == 0)
            {
                y = x;
                x = 1;
            }

            code = GetNextCode(code);
            if (x == tx && y == ty)
            {
                break;
            }
        }

        return code;
    }

    private static ulong GetNextCode(ulong prev)
    {
        return 252533UL * prev % 33554393UL;
    }
}