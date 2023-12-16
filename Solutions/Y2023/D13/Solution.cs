using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D13;

[PuzzleInfo("Point of Incidence", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var grids = GetInputLines()
            .ChunkBy(line => !string.IsNullOrWhiteSpace(line))
            .Select(chunk => Grid2D<bool>.MapChars(chunk, c => c == '#'));
        
        return part switch
        {
            1 => grids.Sum(grid => ScoreSymmetry(grid, exceptions: 0)),
            2 => grids.Sum(grid => ScoreSymmetry(grid, exceptions: 1)),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int ScoreSymmetry(Grid2D<bool> grid, int exceptions)
    {
        if (FindSymmetryVertical(grid, exceptions, out var rawCol))
        {
            return rawCol + 1;
        }
        
        grid.Rotate(deg: Degrees.P90);
        
        if (FindSymmetryVertical(grid, exceptions, out var transformedCol))
        {
            return 100 * (transformedCol + 1);
        }

        throw new NoSolutionException();
    }
    
    private static bool FindSymmetryVertical(Grid2D<bool> grid, int exceptions, out int col)
    {
        for (var x = 0; x < grid.Width - 1; x++)
        {
            if (IsSymmetricalAboutCol(grid, exceptions, col: x))
            {
                col = x;
                return true;
            }
        }

        col = 0;
        return false;
    }

    private static bool IsSymmetricalAboutCol(Grid2D<bool> grid, int exceptions, int col)
    {
        var aberrations = 0;
        
        for (var d = 0; d <= Math.Min(col, grid.Width - col - 2); d++)
        for (var y = 0; y < grid.Height; y++)
        {
            if (grid[col - d, y] != grid[col + d + 1, y] && ++aberrations > exceptions)
            {
                return false;
            }
        }
        
        return aberrations == exceptions;
    }
}