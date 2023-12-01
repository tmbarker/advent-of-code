namespace Utilities.Geometry.Euclidean;

public partial class Grid2D<T>
{
    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> instance by specifying the dimensions
    /// </summary>
    /// <param name="rows">The number of rows in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <param name="cols">The number of columns in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <param name="origin">Which origin should be used</param>
    /// <returns>A <see cref="Grid2D{T}"/> instance with the specified numbers of rows and columns</returns>
    public static Grid2D<T> WithDimensions(int rows, int cols, Origin origin = Origin.Xy)
    {
        return new Grid2D<T>(new T[rows, cols], origin);
    }

    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> from an <see cref="IList{T}"/> collection of <see cref="string"/> by applying
    /// a delegate to each <see cref="char"/> in the input <paramref name="strings"/>
    /// </summary>
    /// <param name="strings">The string collection to populate the returned <see cref="Grid2D{T}"/> instance with</param>
    /// <param name="elementFunc">A delegate which builds an element from a <see cref="char"/></param>
    /// <param name="origin">Which origin should be used</param>
    /// <returns>A populated <see cref="Grid2D{T}"/> instance</returns>
    public static Grid2D<T> MapChars(IList<string> strings, Func<char, T> elementFunc, Origin origin = Origin.Xy)
    {
        var rows = strings.Count;
        var cols = strings[0].Length;
        var array = new T[rows, cols];

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var chr = strings[rows - y - 1][x];
            var element = elementFunc(chr);
            
            array[y, x] = element;
        }

        return new Grid2D<T>(array, origin);
    }
}