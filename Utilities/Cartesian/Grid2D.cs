namespace Utilities.Cartesian;

/// <summary>
/// 2D grid data structure providing (X,Y) style indexing
/// </summary>
/// <typeparam name="T">The type at each grid position</typeparam>
public partial class Grid2D<T>
{
    private const string OutOfRangeFormat = "Index out of range [{0}], must be in range [0-{1}]";

    private readonly T[,]? _grid;
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
        get => Get(x, y);
        set => Set(x, y, value);
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
    /// Get the element at position (x,y)
    /// </summary>
    /// <param name="position">The position to index</param>
    /// <returns>The element at the specified position</returns>
    public T Get(Vector2D position)
    {
        return Get(position.X, position.Y);
    }

    /// <summary>
    /// Set the element at position (x,y)
    /// </summary>
    /// <param name="position">The position to index</param>
    /// <param name="value">The element value to set</param>
    public void Set(Vector2D position, T value)
    {
        Set(position.X, position.Y, value);
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

    private T Get(int x, int y)
    {
        ValidateIndices(x, TransformY(y));
        return _grid![y, x];
    }
    
    private void Set(int x, int y, T value)
    {
        ValidateIndices(x, y);
        _grid![TransformY(y), x] = value;
    }

    private int TransformY(int y)
    {
        return _origin == Origin.Xy ? y : Height - y - 1;
    }
    
    private void EvaluateDimensions()
    {
        Height = _grid!.GetLength(0);
        Width = _grid!.GetLength(1);
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
}