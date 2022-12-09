namespace Utilities.DataStructures.Grid;

public partial class Grid2D<T>
{
    
    /// <summary>
    /// Generate a <see cref="Grid2D{T}"/> instance by specifying the dimensions
    /// </summary>
    /// <param name="rows">The number of rows in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <param name="cols">The number of columns in the resulting <see cref="Grid2D{T}"/> instance</param>
    /// <returns>A <see cref="Grid2D{T}"/> instance with the specified numbers of rows and columns</returns>
    public static Grid2D<T> WithDimensions(int rows, int cols)
    {
        return new Grid2D<T>(new T[rows, cols]);
    }
}