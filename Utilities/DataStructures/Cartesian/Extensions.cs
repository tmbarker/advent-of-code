namespace Utilities.DataStructures.Cartesian;

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
        return Vector2D.ChebyshevDistance(lhs, rhs) <= 1;
    }
    
    /// <summary>
    /// Determine if two positions are edge adjacent, where edge adjacent means the Taxicab distance is less than or equal to 1
    /// </summary>
    public static bool IsEdgeAdjacentTo(this IPosition2D lhs, IPosition2D rhs)
    {
        return Vector2D.TaxicabDistance(lhs, rhs) <= 1;
    }

    /// <summary>
    /// Get a set of vectors adjacent to <paramref name="vector"/>, depending on the <paramref name="metric"/> diagonally
    /// adjacent vectors may or may not be included in the returned set
    /// </summary>
    public static ISet<Vector2D> GetAdjacentSet(this Vector2D vector, DistanceMetric metric)
    {
        var set = new HashSet<Vector2D>
        {
            vector + Vector2D.Up,
            vector + Vector2D.Down, 
            vector + Vector2D.Left,
            vector + Vector2D.Right
        };

        if (metric != DistanceMetric.Chebyshev)
        {
            return set;
        }
        
        set.Add(vector + new Vector2D(1, 1));
        set.Add(vector + new Vector2D(-1, 1));
        set.Add(vector + new Vector2D(-1, -1));
        set.Add(vector + new Vector2D(1, -1));

        return set;
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