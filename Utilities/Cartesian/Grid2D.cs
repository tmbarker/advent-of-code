using System.Collections;

namespace Utilities.Cartesian;

/// <summary>
/// 2D grid data structure providing (X,Y) style indexing
/// </summary>
/// <typeparam name="T">The type at each grid position</typeparam>
public partial class Grid2D<T> : IEnumerable<KeyValuePair<Vector2D, T>>
{
    private const string OutOfRangeFormat = "Index out of range [{0}], must be in range [0-{1}]";
    private const string InvalidFlipAxisError = $"{nameof(Grid2D<T>)} can only be flipped about the X and Y axis";
    private const string InvalidRotAxisError = $"{nameof(Grid2D<T>)} can only be rotated about the Z axis";

    private T[,] _grid;
    private readonly Origin _origin;
    
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
        get => Get(position);
        set => Set(position, value);
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

    public void Flip(Axis about)
    {
        if (about is Axis.Z or Axis.W)
        {
            throw new ArgumentOutOfRangeException(nameof(about), about, InvalidFlipAxisError);
        }

        var tmp = new T[Height, Width];
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (about)
            {
                case Axis.X:
                    tmp[y, x] = _grid[Height - y - 1, x];
                    break;
                case Axis.Y:
                    tmp[y, x] = _grid[y, Width - x - 1];
                    break;
            }
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
    /// Get the element at position (x,y)
    /// </summary>
    /// <param name="position">The position to index</param>
    /// <returns>The element at the specified position</returns>
    private T Get(Vector2D position)
    {
        return GetInternal(position.X, position.Y);
    }

    /// <summary>
    /// Set the element at position (x,y)
    /// </summary>
    /// <param name="position">The position to index</param>
    /// <param name="value">The element value to set</param>
    private void Set(Vector2D position, T value)
    {
        SetInternal(position.X, position.Y, value);
    }

    private T GetInternal(int x, int y)
    {
        ValidateIndices(x, TransformY(y));
        return _grid[y, x];
    }
    
    private void SetInternal(int x, int y, T value)
    {
        ValidateIndices(x, y);
        _grid[TransformY(y), x] = value;
    }

    private int TransformY(int y)
    {
        return _origin == Origin.Xy ? y : Height - y - 1;
    }
    
    private void EvaluateDimensions()
    {
        Height = _grid.GetLength(0);
        Width = _grid.GetLength(1);
    }

    private void ValidateIndices(int x, int y)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(x), x, string.Format(OutOfRangeFormat, x, Width - 1));
        }
        
        if (y < 0 || y >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(y), y, string.Format(OutOfRangeFormat, y, Height - 1));
        }
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