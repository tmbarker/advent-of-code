using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Utilities.Tests.Geometry.Euclidean;

/// <summary>
///     Tests associated with the factory members of <see cref="Grid2D{T}"/>.
/// </summary>
public class Grid2DFactoryTests
{
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
    public void WithDimensions_CreatesGridWithCorrectSize()
    {
        // Arrange
        const int rows = 4;
        const int cols = 5;
        
        // Act
        var grid = Grid2D<int>.WithDimensions(rows, cols);

        // Assert
        Assert.Equal(rows, grid.Height);
        Assert.Equal(cols, grid.Width);
        
        for (var r = 0; r < rows; r++)
        for (var c = 0; c < cols; c++)
        {
            Assert.Equal(default, grid[c, r]);
        }
    }
    
    [Fact]
    public void MapChars_HandlesDefaultCharMapping()
    {
        // Arrange
        var strings = new List<string>
        {
            "abc",
            "def",
            "ghi"
        };

        // Act
        var grid = Grid2D<char>.MapChars(strings);
        var expected = new[,]
        {
            { 'g', 'h', 'i' },
            { 'd', 'e', 'f' },
            { 'a', 'b', 'c' }
        };

        // Assert
        AssertGridEqual(expected, grid);
    }
    
    [Fact]
    public void MapChars_MapChars_HandlesDelegatedCharMapping_XyOrigin()
    {
        // Arrange
        var strings = new List<string>
        {
            "123",
            "456",
            "789"
        };

        // Act
        var grid = Grid2D<int>.MapChars(strings, c => c.AsDigit());
        var expected = new[,]
        {
            { 7, 8, 9 },
            { 4, 5, 6 },
            { 1, 2, 3 }
        };

        // Assert
        AssertGridEqual(expected, grid);
    }
    
    [Fact]
    public void MapChars_HandlesDelegatedCharMapping_UvOrigin()
    {
        // Arrange
        var strings = new List<string>
        {
            "789",
            "456",
            "123"
        };

        // Act
        var grid = Grid2D<int>.MapChars(strings, c => c.AsDigit(), origin: Origin.Uv);
        var expected = new[,]
        {
            { 7, 8, 9 },
            { 4, 5, 6 },
            { 1, 2, 3 }
        };

        // Assert
        AssertGridEqual(expected, grid);
    }

    [Fact]
    public void MapChars_ThrowsForJaggedStrings()
    {
        // Arrange
        var strings = new List<string>
        {
            "123",
            "45",
            "789"
        };
        
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => Grid2D<int>.MapChars(strings, c => c.AsDigit()));
    }
    
}