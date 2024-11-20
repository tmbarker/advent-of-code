using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A 2D grid data structure providing (X,Y) style indexing
/// </summary>
/// <typeparam name="T">The type associated with the value at each grid position</typeparam>
public sealed partial class Grid2D<T> : IEnumerable<Vec2D>
{
    /// <summary>
    ///     The internal backing array, which is indexed from the bottom left.
    ///     <para />
    ///     When <see cref="_origin" /> is set to <see cref="Origin.Xy" /> the position argument of "Get" and "Set"
    ///     queries will match the array index, e.g. <see cref="Vec2D" />.<see cref="Vec2D.Zero" /> will index the
    ///     bottom left element from the array (i.e. <see cref="_array" />[0, 0]).
    ///     <para />
    ///     Accordingly, when <see cref="_origin" /> is set to <see cref="Origin.Uv" /> then
    ///     <see cref="Vec2D" />.<see cref="Vec2D.Zero" /> will index the top left element from the array.
    /// </summary>
    private T[,] _array;

    private readonly Origin _origin;

    /// <summary>
    ///     Internal constructor
    /// </summary>
    /// <param name="array">
    ///     The 2D array which will back the Grid instance, it should be populated such that the bottom
    ///     left element of the <see cref="Grid2D{T}" /> is indexed at [0,0] when using the <see cref="Origin.Xy" /> origin
    /// </param>
    /// <param name="origin">Which origin to use</param>
    private Grid2D(T[,] array, Origin origin)
    {
        _array = array;
        _origin = origin;
    }

    /// <summary>
    ///     The number of columns in the <see cref="Grid2D{T}" /> instance
    /// </summary>
    public int Width => _array.GetLength(dimension: 1);

    /// <summary>
    ///     The number of rows in the <see cref="Grid2D{T}" /> instance
    /// </summary>
    public int Height => _array.GetLength(dimension: 0);

    /// <summary>
    ///     Index the element at position (<paramref name="x" />, <paramref name="y" />)
    /// </summary>
    /// <param name="x">The column index</param>
    /// <param name="y">The row index</param>
    public T this[int x, int y]
    {
        get => GetElementInternal(x, y);
        set => SetElementInternal(x, y, value);
    }

    /// <summary>
    ///     Index the element at <paramref name="position" />
    /// </summary>
    /// <param name="position">The position to index</param>
    public T this[Vec2D position]
    {
        get => GetElementInternal(position.X, position.Y);
        set => SetElementInternal(position.X, position.Y, value);
    }

    /// <summary>
    ///     Check if a position is within the bounds of the <see cref="Grid2D{T}" /> instance
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>A Boolean representing if the position is within the bounds of the <see cref="Grid2D{T}" /></returns>
    public bool Contains(Vec2D position)
    {
        return
            position.X >= 0 && position.X < Width &&
            position.Y >= 0 && position.Y < Height;
    }

    /// <summary>
    ///     Print the <see cref="Grid2D{T}" /> instance contents to the console
    /// </summary>
    public void Print(Func<Vec2D, T, string>? elementFormatter = null, int padding = 0)
    {
        Console.Write(BuildRepresentativeString(elementFormatter, padding));
    }

    /// <summary>
    ///     Generate a representative string of the <see cref="Grid2D{T}" /> instance based on its current contents
    /// </summary>
    public string BuildRepresentativeString(Func<Vec2D, T, string>? elementFormatter = null, int padding = 0,
        string? prepend = null)
    {
        var sb = new StringBuilder();
        var paddingStr = padding > 0
            ? new string(c: ' ', padding)
            : string.Empty;

        if (!string.IsNullOrEmpty(prepend))
        {
            sb.Append(prepend);
        }

        for (var r = Height - 1; r >= 0; r--)
        {
            for (var c = 0; c < Width; c++)
            {
                var element = _array[r, c];
                var (x, y) = ArrayIndicesToXy(row: r, col: c);
                var pos = new Vec2D(x, y);

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
    ///     Extract a row from the grid
    /// </summary>
    /// <param name="rowIndex">The 0-based row index</param>
    /// <returns>The elements of the row, starting with the 0-index column element</returns>
    public IEnumerable<T> EnumerateRow(int rowIndex)
    {
        for (var x = 0; x < Width; x++)
        {
            yield return GetElementInternal(x, y: rowIndex);
        }
    }

    /// <summary>
    ///     Extract a column from the grid
    /// </summary>
    /// <param name="colIndex">The 0-based column index</param>
    /// <returns>The elements of the column, starting with the 0-index row element</returns>
    public IEnumerable<T> EnumerateCol(int colIndex)
    {
        for (var y = 0; y < Height; y++)
        {
            yield return GetElementInternal(x: colIndex, y);
        }
    }

    private T GetElementInternal(int x, int y)
    {
        var (row, col) = XyToArrayIndices(x, y);
        return _array[row, col];
    }

    private void SetElementInternal(int x, int y, T value)
    {
        var (row, col) = XyToArrayIndices(x, y);
        _array[row, col] = value;
    }

    /// <summary>
    ///     Convert the abstracted XY position of a grid element to indices of the backing array
    /// </summary>
    /// <param name="x">The X component of the abstracted grid element position</param>
    /// <param name="y">The y component of the abstracted grid element position</param>
    /// <returns>The row and col values used to index into the backing array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (int row, int col) XyToArrayIndices(int x, int y)
    {
        var row = _origin == Origin.Xy ? y : Height - y - 1;
        var col = x;

        return (row, col);
    }

    /// <summary>
    ///     Convert backing array indices to the corresponding abstracted XY position of a grid element
    /// </summary>
    /// <param name="row">The row index in the backing array</param>
    /// <param name="col">The col index in the backing array</param>
    /// <returns>The XY position of the abstracted grid element</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (int x, int y) ArrayIndicesToXy(int row, int col)
    {
        var x = col;
        var y = _origin == Origin.Xy ? row : Height - row - 1;

        return (x, y);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Vec2D> GetEnumerator()
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            yield return new Vec2D(x, y);
        }
    }
}