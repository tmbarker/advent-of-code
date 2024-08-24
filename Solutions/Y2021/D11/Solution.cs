using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D11;

[PuzzleInfo("Dumbo Octopus", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var state = Grid2D<int>.MapChars(strings: input, elementFunc:StringExtensions.AsDigit);
        var grid = new OctopusGrid(state);
        
        return part switch
        {
            1 => grid.CountFlashes(steps: 100, type: OctopusGrid.FlashType.Single),
            2 => grid.CountStepsUntilFlash(type: OctopusGrid.FlashType.All), 
            _ => PuzzleNotSolvedString
        };
    }
}