using Problems.Common;

namespace Problems.Y2015.D01;

/// <summary>
/// Not Quite Lisp: https://adventofcode.com/2015/day/1
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var instructions = GetInputText();
        return part switch
        {
            1 => FollowInstructions(instructions, basement: false),
            2 => FollowInstructions(instructions, basement: true),
            _ => ProblemNotSolvedString
        };
    }

    private static int FollowInstructions(string instructions, bool basement)
    {
        var floor = 0;
        var index = 0;
        
        while (index < instructions.Length)
        {
            floor += instructions[index++] == '(' ? 1 : -1;
            
            if (basement && floor == -1)
            {
                return index;
            }
        }

        return floor;
    }
}