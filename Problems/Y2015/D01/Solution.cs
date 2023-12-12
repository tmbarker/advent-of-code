namespace Problems.Y2015.D01;

/// https://adventofcode.com/2015/day/1
[PuzzleInfo("Not Quite Lisp", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
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