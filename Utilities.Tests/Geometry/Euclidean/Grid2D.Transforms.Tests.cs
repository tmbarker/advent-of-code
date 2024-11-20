using Utilities.Geometry.Euclidean;

namespace Utilities.Tests.Geometry.Euclidean;

/// <summary>
///     Tests associated with transform-focused members of <see cref="Grid2D{T}"/>. 
/// </summary>
public class Grid2DTransformsTests
{
    private static Grid2D<int> CreateTestGrid(int width, int height, Origin origin)
    {
        var grid = Grid2D<int>.WithDimensions(rows: height, cols: width, origin);
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            grid[x, y] = y * width + x;
        }
        
        return grid;
    }
    
    private static void AssertGridEqual<T>(T[,] expected, Grid2D<T> actual)
    {
        Assert.Equal(expected.GetLength(dimension: 0), actual.Height);
        Assert.Equal(expected.GetLength(dimension: 1), actual.Width);

        for (var y = 0; y < actual.Height; y++)
        for (var x = 0; x < actual.Width; x++)
        {
            Assert.Equal(expected[y, x], actual[x, y]); // x = col, y = row
        }
    }

    [Fact]
    public void Flip_FlipsVerticallyCorrectly()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 3, Origin.Xy);
        var expected = new[,] { { 6, 7, 8 }, { 3, 4, 5 }, { 0, 1, 2 } };
        
        // Act
        grid.Flip(about: Axis.X);
        
        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void Flip_FlipsHorizontallyCorrectly()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 3, Origin.Xy);
        var expected = new[,] { { 2, 1, 0 }, { 5, 4, 3 }, { 8, 7, 6 } };
        
        // Act
        grid.Flip(about: Axis.Y);
        
        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void Flip_ThrowsForInvalidAxis()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 3, Origin.Xy);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => grid.Flip(about: Axis.Z));
        Assert.Throws<ArgumentOutOfRangeException>(() => grid.Flip(about: Axis.W));
    }

    [Fact]
    public void Rotate_Rotates90DegreesCcw()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 2, Origin.Xy);
        var expected = new[,] { { 3, 0 }, { 4, 1 }, { 5, 2 } };
        
        // Act
        grid.Rotate(deg: Degrees.P90);
        
        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void Rotate_Rotates180Degrees()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 2, Origin.Xy);
        var expected = new[,] { { 5, 4, 3 }, { 2, 1, 0 } };
        
        // Act
        grid.Rotate(deg: Degrees.P180);
        
        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void Rotate_Rotates90DegreesCw()
    {
        // Arrange
        var grid = CreateTestGrid(width: 3, height: 2, Origin.Xy);
        var expected = new[,] { { 2, 5 }, { 1, 4 }, { 0, 3 } };
        
        // Act
        grid.Rotate(deg: Degrees.N90);
        
        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void Rotate_Rotates360Degrees_NoChange()
    {
        // Arrange
        var original = CreateTestGrid(width: 3, height: 2, Origin.Xy);
        var grid = CreateTestGrid(width: 3, height: 2, Origin.Xy);

        // Act
        grid.Rotate(deg: Degrees.P360);

        // Assert
        Assert.True(grid.SequenceEqual(original));
    }

    [Fact]
    public void Rotate_ThrowsForInvalidAngle()
    {
        // Arrange
        var grid = CreateTestGrid(width:3, height:2, Origin.Xy);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => grid.Rotate(deg: 45));
        Assert.Throws<ArgumentOutOfRangeException>(() => grid.Rotate(deg: 275));
    }
    
}