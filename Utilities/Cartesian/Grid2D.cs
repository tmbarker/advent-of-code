using System.Collections;
using System.Text;

namespace Utilities.Cartesian;

/// <summary>
/// 2D grid data structure providing (X,Y) style indexing
/// </summary>
/// <typeparam name="T">The type at each grid position</typeparam>
public partial class Grid2D<T> : IEnumerable<KeyValuePair<Vector2D, T>>
{
    private const string InvalidFlipAxisError = $"{nameof(Grid2D<T>)} can only be flipped about the X and Y axis";
    private const string InvalidRotAxisError = $"{nameof(Grid2D<T>)} can only be rotated about the Z axis";
    
    private T[,] _grid;
    private readonly Origin _origin;
    
    /// <summary>
    /// Internal constructor
    /// </summary>
    /// <param name="grid">The 2D array which will back the Grid instance, it should be populated such that the bottom
    /// left element of the <see cref="Grid2D{T}"/> is indexed at [0,0] when using the <see cref="Origin.Xy"/> origin</param>
    /// <param name="origin">Which origin to use</param>
    private Grid2D(T[,] grid, Origin origin)
    {
        _grid = grid;
        _origin = origin;
        
        EvaluateDimensions();
    }
    
    /// <summary>
    /// The number of columns in the <see cref="Grid2D{T}"/> instance
    /// </summary>
    public int Width { get; private set; }
    /// <summary>
    /// The number of rows in the <see cref="Grid2D{T}"/> instance
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Index the element at position (<paramref name="x"/>,<paramref name="y"/>)
    /// </summary>
    /// <param name="x">The column index</param>
    /// <param name="y">The row index</param>
    public T this[int x, int y]
    {
        get => GetInternal(x, y);
        set => SetInternal(x, y, value);
    }

    /// <summary>
    /// Index the element at <paramref name="position"/>
    /// </summary>
    /// <param name="position">The position to index</param>
    public T this[Vector2D position]
    {
        get => GetInternal(position.X, position.Y);
        set => SetInternal(position.X, position.Y, value);
    }

    /// <summary>
    /// Check if a position is within the bounds of the <see cref="Grid2D{T}"/> instance
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>A Boolean representing if the position is within the bounds of the <see cref="Grid2D{T}"/></returns>
    public bool IsInDomain(Vector2D position)
    {
        return
            position.X >= 0 && position.X < Width &&
            position.Y >= 0 && position.Y < Height;
    }
    
    /// <summary>
    /// Print the <see cref="Grid2D{T}"/> instance contents to the console
    /// </summary>
    public void Print(Func<Vector2D, T, string>? elementFormatter = null, int padding = 0)
    {
        Console.Write(BuildRepresentativeString(elementFormatter, padding));
    }

    /// <summary>
    /// Generate a representative string of the <see cref="Grid2D{T}"/> instance based on its current contents
    /// </summary>
    public string BuildRepresentativeString(Func<Vector2D, T, string>? elementFormatter = null, int padding = 0,
        string? prepend = null)
    {
        var sb = new StringBuilder();
        var paddingStr = padding > 0 
            ? new string(' ', padding) 
            : string.Empty;

        if (!string.IsNullOrEmpty(prepend))
        {
            sb.Append(prepend);
        }
        
        for (var y = Height - 1; y >= 0; y--)
        {
            for (var x = 0; x < Width; x++)
            {
                var element = _grid[y, x];
                var pos = new Vector2D(
                    x: x,
                    y: TransformY(y));
                
                var elementString = elementFormatter != null
                    ? elementFormatter(pos, element)
                    : element?.ToString();
                    
                sb.Append($"{elementString}{paddingStr}");
            }
            sb.Append('\n');
        }

        return sb.ToString();
    }
    
    /// <summary>
    /// Get all positions in the <see cref="Grid2D{T}"/> instance
    /// </summary>
    public IEnumerable<Vector2D> GetAllPositions()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            yield return new Vector2D(x, y);
        }
    }

    /// <summary>
    /// Flip the grid about the specified axis
    /// </summary>
    /// <param name="about">The axis about which to flip the grid</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Axis.X"/> and <see cref="Axis.Y"/> axes only</exception>
    public void Flip(Axis about)
    {
        var tmp = new T[Height, Width];
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            tmp[y, x] = about switch
            {
                Axis.X => _grid[Height - y - 1, x],
                Axis.Y => _grid[y, Width - x - 1],
                _ => throw new ArgumentOutOfRangeException(nameof(about), about, InvalidFlipAxisError)
            };
        }

        _grid = tmp;
        EvaluateDimensions();
    }
    
    /// <summary>
    /// Rotate the grid by the given argument
    /// </summary>
    /// <param name="rot">The rotation to perform on the grid</param>
    /// <exception cref="ArgumentOutOfRangeException">Argument must be a rotation around the <see cref="Axis.Z"/> axis</exception>
    public void Rotate(Rotation3D rot)
    {
        if (rot.Axis != Axis.Z)
        {
            throw new ArgumentOutOfRangeException(nameof(rot), rot, InvalidRotAxisError);
        }
        
        var map = new Dictionary<Vector2D, Vector2D>();
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            var from = new Vector2D(x, y);
            var to = rot * from;
            map[to] = from;
        }

        var xMin = map.Keys.Min(v => v.X);
        var xMax = map.Keys.Max(v => v.X);
        var yMin = map.Keys.Min(v => v.Y);
        var yMax = map.Keys.Max(v => v.Y);

        var tmp = new T[yMax - yMin + 1, xMax - xMin + 1];
        foreach (var ((xTo, yTo), (xFrom, yFrom)) in map)
        {
            tmp[yTo - yMin, xTo - xMin] = _grid[yFrom, xFrom];
        }

        _grid = tmp;
        EvaluateDimensions();
    }
    
    /// <summary>
    /// Extract a row from the grid
    /// </summary>
    /// <param name="rowIndex">The 0-based row index</param>
    /// <returns>The elements of the row, starting with the 0-index column element</returns>
    public IEnumerable<T> GetRow(int rowIndex)
    {
        for (var x = 0; x < Width; x++)
        {
            yield return GetInternal(x, rowIndex);
        }
    }
    
    /// <summary>
    /// Extract a column from the grid
    /// </summary>
    /// <param name="colIndex">The 0-based column index</param>
    /// <returns>The elements of the column, starting with the 0-index row element</returns>
    public IEnumerable<T> GetCol(int colIndex)
    {
        for (var y = 0; y < Height; y++)
        {
            yield return GetInternal(colIndex, y);
        }
    }
    
    /// <summary>
    /// Extract the positions of a row from the grid
    /// </summary>
    /// <param name="rowIndex">The 0-based row index</param>
    /// <returns>The position elements of the row, starting with the 0-index column element</returns>
    public IEnumerable<Vector2D> GetRowPositions(int rowIndex)
    {
        for (var x = 0; x < Width; x++)
        {
            yield return new Vector2D(x, rowIndex);
        }
    }
    
    /// <summary>
    /// Extract the positions of a column from the grid
    /// </summary>
    /// <param name="colIndex">The 0-based column index</param>
    /// <returns>The position elements of the column, starting with the 0-index row element</returns>
    public IEnumerable<Vector2D> GetColPositions(int colIndex)
    {
        for (var y = 0; y < Height; y++)
        {
            yield return new Vector2D(colIndex, y);
        }
    }

    private T GetInternal(int x, int y)
    {
        return _grid[TransformY(y), x];
    }
    
    private void SetInternal(int x, int y, T value)
    {
        _grid[TransformY(y), x] = value;
    }

    private int TransformY(int raw)
    {
        return _origin == Origin.Xy ? raw : Height - raw - 1;
    }
    
    private void EvaluateDimensions()
    {
        Height = _grid.GetLength(0);
        Width = _grid.GetLength(1);
    }

    public IEnumerator<KeyValuePair<Vector2D, T>> GetEnumerator()
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            yield return new KeyValuePair<Vector2D, T>(new Vector2D(x, y), GetInternal(x, y));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}