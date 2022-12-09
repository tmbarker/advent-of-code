using Problems.Y2022.Common;

namespace Problems.Y2022.D10;

public class Solution : SolutionBase2022
{
    public override int Day => 10;
    
    public override string Run(int part)
    {
        AssertInputExists();

        return part switch
        {
            _ => ProblemNotSolvedString,
        };
    }
}