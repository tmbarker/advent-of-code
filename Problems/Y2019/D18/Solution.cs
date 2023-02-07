using Problems.Y2019.Common;

namespace Problems.Y2019.D18;

/// <summary>
/// Many-Worlds Interpretation: https://adventofcode.com/2019/day/18
/// </summary>
public class Solution : SolutionBase2019
{ 
    public override int Day => 18;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => FindShortestPath(false),
            _ => ProblemNotSolvedString
        };
    }

    private int FindShortestPath(bool applyInputOverrides)
    {
        var field = Field.Parse(GetInputLines(), applyInputOverrides);
        var pathFinder = new PathFinder(field);

        return pathFinder.Run();
    }
}