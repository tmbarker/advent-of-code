namespace Utilities.DataStructures.Grid;

public static class Extensions
{
    /// <summary>
    /// Determine if two positions are diagonal, where they do not share a common value for either dimension
    /// </summary>
    public static bool IsDiagonalTo(this IPosition2D lhs, IPosition2D rhs)
    {
        return lhs.X != rhs.X && lhs.Y != rhs.Y;
    }
    
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the Chebyshev distance is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this IPosition2D lhs, IPosition2D rhs)
    {
        return ChebyshevDistance(lhs, rhs) <= 1;
    }

    /// <summary>
    /// Compute the Chebyshev distance, which is also known as the Chessboard distance
    /// </summary>
    public static int ChebyshevDistance(this IPosition2D lhs, IPosition2D rhs)
    {
        var dx = Math.Abs(lhs.X - rhs.X);
        var dy = Math.Abs(lhs.Y - rhs.Y);

        return Math.Max(dx, dy);
    }

    /// <summary>
    /// Print Grid contents to the console
    /// </summary>
    public static void Print<T>(this Grid2D<T> grid, string title = "GRID", Func<IPosition2D, T, string>? elementFormatter = null)
    {
        const int defaultTabStopWidth = 8;
        const char headerChar = '-';
        
        var headerAffix = new string(headerChar, defaultTabStopWidth * (grid.Width / 2));
        
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
                    
                Console.Write($"{elementString}\t");
            }
            Console.WriteLine();
        }
    }
}