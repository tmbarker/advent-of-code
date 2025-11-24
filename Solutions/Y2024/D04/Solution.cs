using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D04;

[PuzzleInfo("Ceres Search", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private static readonly char[] Xmas = ['X', 'M', 'A', 'S'];
    private static readonly Vec2D[] D1 = [new(-1,  1), new(1, -1)];
    private static readonly Vec2D[] D2 = [new(-1, -1), new(1,  1)];
    
    public override object Run(int part)
    {
        var grid = GetInputGrid();
        return part switch
        {
            1 => CountLinear(grid),
            2 => CountCrossed(grid),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountLinear(Grid2D<char> grid)
    {
        var dirs = Vec2D.Zero.GetAdjacentSet(Metric.Chebyshev);
        return grid
            .Where(pos => grid[pos] == 'X')
            .Sum(candidate => dirs.Count(dir => CheckLinear(grid, start: candidate, dir)));
    }
    
    private static bool CheckLinear(Grid2D<char> grid, Vec2D start, Vec2D dir)
    {
        return Xmas
            .Select((c, i) => (Chr: c, Pos: start + i * dir))
            .All(tuple => grid.Contains(tuple.Pos) && grid[tuple.Pos] == tuple.Chr);
    }
    
    private static int CountCrossed(Grid2D<char> grid)
    {
        return grid
            .Where(pos => grid[pos] == 'A')
            .Count(candidate => CheckCrossed(grid, center: candidate));
    }

    private static bool CheckCrossed(Grid2D<char> grid, Vec2D center)
    {
        return IsMas(grid, center, diag: D1) &&
               IsMas(grid, center, diag: D2);
    }

    private static bool IsMas(Grid2D<char> grid, Vec2D center, Vec2D[] diag)
    {
        var c1 = center + diag[0];
        var c2 = center + diag[1];

        if (!grid.Contains(c1) || !grid.Contains(c2)) return false;
        return (grid[c1] == 'M' && grid[c2] == 'S') ||
               (grid[c1] == 'S' && grid[c2] == 'M');
    }
}