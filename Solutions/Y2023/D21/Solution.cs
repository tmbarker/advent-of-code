using Utilities.Extensions;
using Utilities.Geometry.Euclidean;
using Utilities.Numerics;

namespace Solutions.Y2023.D21;

[PuzzleInfo("Step Counter", Topics.Vectors|Topics.Math, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const char Start = 'S';
    private const char Empty = '.';
    
    public override object Run(int part)
    {
        var grid = Grid2D<char>.MapChars(strings: GetInputLines());
        var start = grid.Single(pos => grid[pos] == Start);
        
        grid[start] = Empty;
        
        return part switch
        {
            1 => Simulate(grid, start, sampleAt: [64])[0],
            2 => Extrapolate(grid, start, n: 26501365),
            _ => PuzzleNotSolvedString
        };
    }

    private static List<long> Simulate(Grid2D<char> grid, Vec2D start, HashSet<int> sampleAt)
    {
        var ticks = new List<long>(capacity: sampleAt.Count);
        var heads = new HashSet<Vec2D>(collection: [start]);
        var after = new HashSet<Vec2D>();
        var memo =  new Dictionary<Vec2D, ISet<Vec2D>>();
        
        for (var i = 1; i <= sampleAt.Max(); i++)
        {
            after.Clear();
            
            foreach (var pos in heads)
            foreach (var adj in GetEmptyAdjacent(grid, pos, memo))
            {
                after.Add(adj);
            }
            
            if (sampleAt.Contains(i))
            {
                ticks.Add(after.Count);
            }

            (heads, after) = (after, heads);
        }

        return ticks;
    }
    
    private static long Extrapolate(Grid2D<char> grid, Vec2D start, int n)
    {
        //  The input has a number of properties which together allow for an analytic
        //  solution without the need for purely naive simulation: 
        //    - The grid is square
        //    - The start is in the center of the grid
        //    - The row and column running through the center of the grid are entirely empty
        //    - After reaching the boundary of the first grid, which due to the above constraint
        //      happens in exactly <grid size / 2> steps, the remaining number of steps to
        //      simulate has no residue modulo the grid size
        //
        var x0 = grid.Width / 2;
        var dx = grid.Width;
        var ys = Simulate(grid, start, sampleAt: [x0, x0 + dx, x0 + 2 * dx]);
        
        //  Solve a system of equations to obtain the quadratic coefficients a, b, and c:
        //  (1) c = y0, (2) a + b + c = y1, (3) 4a + 2b + c = y2
        //
        var coefficients = LinearSolver.Solve(a: new double[,]
        {
            { 4, 2, 1, ys[2]},
            { 1, 1, 1, ys[1]},
            { 0, 0, 1, ys[0]}
        });

        var a = (long)coefficients[0];
        var b = (long)coefficients[1];
        var c = (long)coefficients[2];
        var x = (n - x0) / dx;
        
        return a * x * x + b * x + c;
    }
    
    private static IEnumerable<Vec2D> GetEmptyAdjacent(Grid2D<char> grid, Vec2D pos,
        IDictionary<Vec2D, ISet<Vec2D>> memo)
    {
        if (memo.TryGetValue(pos, out var cached))
        {
            return cached;
        }
        
        var set = new HashSet<Vec2D>();
        foreach (var naive in pos.GetAdjacentSet(Metric.Taxicab))
        {
            var adj = new Vec2D(
                x: naive.X.Modulo(grid.Width),
                y: naive.Y.Modulo(grid.Height));
            
            if (grid.Contains(adj) && grid[adj] == Empty)
            {
                set.Add(naive);
            }
        }

        memo[pos] = set;
        return set;
    }
}