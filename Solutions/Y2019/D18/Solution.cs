namespace Solutions.Y2019.D18;

[PuzzleInfo("Many-Worlds Interpretation", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
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