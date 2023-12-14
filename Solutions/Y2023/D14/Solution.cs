using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D14;

[PuzzleInfo("Parabolic Reflector Dish", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Rock = 'O';
    private const char Void = '.';
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var grid = Grid2D<char>.MapChars(lines, c => c);

        return part switch
        {
            1 => ComputeLoad(grid: Tilt(grid, d: Vector2D.Up)),
            2 => ComputeLoad(grid: Spin(grid, n: 1000000000L)),
            _ => ProblemNotSolvedString
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
    
    private static Grid2D<char> Tilt(Grid2D<char> grid, Vector2D d)
    {
        var rocks = grid.Where(pos => grid[pos] == Rock);
        var order = d switch
        {
            { } when d == Vector2D.Up    => rocks.OrderByDescending(pos => pos.Y),
            { } when d == Vector2D.Left  => rocks.OrderBy(pos => pos.X),
            { } when d == Vector2D.Down  => rocks.OrderBy(pos => pos.Y),
            { } when d == Vector2D.Right => rocks.OrderByDescending(pos => pos.X),
            { } => throw new NoSolutionException()
        };
        
        foreach (var pos in order)
        {
            Roll(grid, pos, d);
        }
        
        return grid;
    }
    
    private static void Roll(Grid2D<char> grid, Vector2D pos, Vector2D dir)
    {
        var target = pos + dir;
        while (grid.IsInDomain(target) && grid[target] == Void)
        {
            target += dir;
        }

        grid[pos] = Void;
        grid[target - dir] = Rock;
    }
    
    private static void Cycle(Grid2D<char> grid)
    {
        Tilt(grid, d: Vector2D.Up);
        Tilt(grid, d: Vector2D.Left);
        Tilt(grid, d: Vector2D.Down);
        Tilt(grid, d: Vector2D.Right);
    }
}