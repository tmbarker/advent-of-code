using Problems.Y2020.Common;

namespace Problems.Y2020.D14;

/// <summary>
/// Docking Data: https://adventofcode.com/2020/day/14
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 14;
    
    public override object Run(int part)
    {
        var program = GetInputLines();
        return part switch
        { 
            1 =>  Machine.RunV1(program),
            2 =>  Machine.RunV2(program),
            _ => ProblemNotSolvedString
        };
    }
}