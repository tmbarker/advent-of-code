namespace Utilities.Geometry.Euclidean;

public sealed partial class Grid2D<T>
{
    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> instance by specifying the dimensions.
    /// </summary>
    /// <param name="rows">The number of rows in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <param name="cols">The number of columns in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <param name="origin">Which origin should be used</param>
    /// <returns>A <see cref="Grid2D{T}"/> instance with the specified numbers of rows and columns</returns>
    public static Grid2D<T> WithDimensions(int rows, int cols, Origin origin = Origin.Xy)
    {
        return new Grid2D<T>(array: new T[rows, cols], origin);
    }

    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> instance from an <see cref="IList{T}"/> collection of <see cref="string"/>
    /// by applying a delegate to each <see cref="char"/> in the input <paramref name="strings"/>.
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

        for (var row = 0; row < rows; row++)
        for (var col = 0; col < cols; col++)
        {
            //  NOTE: The backing array is always populated such that the bottom left element of the string "grid"
            //  is populated into the bottom left element of the array. Once the Grid2D instance is constructed,
            //  position indexing (i.e. abstracted array indexing) will be subject to the origin convention used.
            //
            var chr = strings[rows - row - 1][col];
            var element = elementFunc(chr);
            
            array[row, col] = element;
        }

        return new Grid2D<T>(array, origin);
    }
    
    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> instance from an <see cref="IList{T}"/> collection of <see cref="string"/>
    /// by taking each <see cref="char"/> as an element.
    /// </summary>
    /// <param name="strings">The string collection to populate the returned <see cref="Grid2D{T}"/> instance with</param>
    /// <param name="origin">Which origin should be used</param>
    /// <returns>A populated <see cref="Grid2D{T}"/> instance</returns>
    public static Grid2D<char> MapChars(IList<string> strings, Origin origin = Origin.Xy)
    {
        return Grid2D<char>.MapChars(strings, elementFunc: c => c, origin);
    }
}