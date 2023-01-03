namespace Utilities.DataStructures.Cartesian;

public static class Extensions
{
    /// <summary>
    /// Determine if two positions are diagonal, where they do not share a common value for either dimension
    /// </summary>
    public static bool IsDiagonalTo(this Vector2D lhs, Vector2D rhs)
    {
        return lhs.X != rhs.X && lhs.Y != rhs.Y;
    }
    
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the specified distance metric is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this Vector2D lhs, Vector2D rhs, DistanceMetric metric)
    {
        return Vector2D.Distance(lhs, rhs, metric) <= 1;
    }
    
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the specified distance metric is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this Vector3D lhs, Vector3D rhs, DistanceMetric metric)
    {
        return Vector3D.Distance(lhs, rhs, metric) <= 1;
    }

    /// <summary>
    /// Get a set of vectors adjacent to <paramref name="vector"/>, depending on the <paramref name="metric"/> diagonally
    /// adjacent vectors may or may not be included in the returned set
    /// </summary>
    /// <exception cref="ArgumentException">This method does not support the Euclidean distance metric</exception>
    public static ISet<Vector2D> GetAdjacentSet(this Vector2D vector, DistanceMetric metric)
    {
        if (metric == DistanceMetric.Euclidean)
        {
            throw new ArgumentException(
                $"The {DistanceMetric.Euclidean} distance metric is not well defined over integral vector space",
                nameof(metric));
        }
        
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
        
        for (var x = -1; x <= 1; x += 2)
        for (var y = -1; y <= 1; y += 2)
        {
            set.Add(vector + new Vector2D(x, y));
        }

        return set;
    }

    /// <summary>
    /// Get a set of vectors adjacent to <paramref name="vector"/>, depending on the <paramref name="metric"/> diagonally
    /// adjacent vectors may or may not be included in the returned set
    /// </summary>
    /// <exception cref="ArgumentException">This method does not support the Euclidean distance metric</exception>
    public static ISet<Vector3D> GetAdjacentSet(this Vector3D vector, DistanceMetric metric)
    {
        if (metric == DistanceMetric.Euclidean)
        {
            throw new ArgumentException(
                $"The {DistanceMetric.Euclidean} distance metric is not well defined over integral vector space",
                nameof(metric));
        }
        
        var set = new HashSet<Vector3D>
        {
            vector + Vector3D.Up,
            vector + Vector3D.Down, 
            vector + Vector3D.Left,
            vector + Vector3D.Right,
            vector + Vector3D.Forward,
            vector + Vector3D.Back,
        };

        if (metric != DistanceMetric.Chebyshev)
        {
            return set;
        }
        
        for (var x = -1; x <= 1; x += 2)
        for (var y = -1; y <= 1; y += 2)
        for (var z = -1; z <= 1; z += 2)
        {
            set.Add(vector + new Vector3D(x, y, z));
        }

        return set;
    }

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