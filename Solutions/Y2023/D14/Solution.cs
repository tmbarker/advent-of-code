using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D14;

[PuzzleInfo("Parabolic Reflector Dish", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Rock = 'O';
    private const char Void = '.';
    
    public override object Run(int part)
    {
        var grid = GetInputGrid();
        return part switch
        {
            1 => ComputeLoad(grid: Tilt(grid)),
            2 => ComputeLoad(grid: Spin(grid, n: 1000000000L)),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int ComputeLoad(Grid2D<char> grid)
    {
        return grid.Where(pos => grid[pos] == Rock).Sum(pos => pos.Y + 1);
    }
    
    private static Grid2D<char> Spin(Grid2D<char> grid, long n)
    {
        var seen = new Dictionary<string, long>();
        var done = -1L;
        
        while (seen.TryAdd(key: grid.BuildRepresentativeString(), value: ++done))
        {
            Cycle(grid);
        }
        
        var cycle = done - seen[grid.BuildRepresentativeString()];
        var delta = (n - done) % cycle;
        
        for (var i = 0; i < delta; i++)
        {
            Cycle(grid);
        }

        return grid;
    }
    
    private static Grid2D<char> Tilt(Grid2D<char> grid)
    {
        for (var y = grid.Height - 1; y >= 0; y--)
        for (var x = 0; x < grid.Width; x++)
        {
            if (grid[x, y] != Rock)
            {
                continue;
            }
            
            var pos = new Vec2D(x, y);
            var target = pos + Vec2D.Up;
            
            while (grid.Contains(target) && grid[target] == Void)
            {
                target += Vec2D.Up;
            }

            grid[pos] = Void;
            grid[target + Vec2D.Down] = Rock;
        }
        
        return grid;
    }
    
    private static void Cycle(Grid2D<char> grid)
    {
        for (var i = 0; i < 4; i++)
        {
            Tilt(grid).Rotate(deg: Degrees.N90);   
        }
    }
}