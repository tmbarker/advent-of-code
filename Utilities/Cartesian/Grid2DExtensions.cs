namespace Utilities.Cartesian;

public static class GridExtensions
{
    /// <summary>
    /// Print Grid contents to the console
    /// </summary>
    public static void Print<T>(this Grid2D<T> grid, string title = "GRID", Func<Vector2D, T, string>? elementFormatter = null, int padding = 4)
    {
        const char headerChar = '-';
        
        var headerAffix = new string(headerChar, padding * (grid.Width / 2));
        var paddingStr = padding > 0 ? new string(' ', 4) : string.Empty;
        
        Console.WriteLine();
        Console.WriteLine($"{headerAffix} {title} {headerAffix}");
        
        for (var y = grid.Height - 1; y >= 0; y--)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                var element = grid[x, y];
                var elementString = elementFormatter != null
                    ? elementFormatter(new Vector2D(x, y), element)
                    : element?.ToString();
                    
                Console.Write($"{elementString}{paddingStr}");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Get all positions in the Grid
    /// </summary>
    public static IEnumerable<Vector2D> GetAllPositions<T>(this Grid2D<T> grid)
    {
        for (var x = 0; x < grid.Width; x++)
        for (var y = 0; y < grid.Height; y++)
        {
            yield return new Vector2D(x, y);
        }
    }
}