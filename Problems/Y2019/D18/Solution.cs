using Problems.Common;

namespace Problems.Y2019.D18;

/// <summary>
/// Many-Worlds Interpretation: https://adventofcode.com/2019/day/18
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => FindShortestPath(applyInputOverrides: false, ignoreDoors: false),
            2 => FindShortestPath(applyInputOverrides: true,  ignoreDoors: true),
            _ => ProblemNotSolvedString
        };
    }

    private int FindShortestPath(bool applyInputOverrides, bool ignoreDoors)
    {
        var fields = Field.Parse(GetInputLines(), applyInputOverrides);
        return fields.Sum(f => new PathFinder(f).Run(ignoreDoors));
    }
}