using Utilities.Geometry.Euclidean;

namespace Utilities.Tests.Geometry.Euclidean;

/// <summary>
///     Tests associated with <see cref="Grid2D{T}"/>.
/// </summary>
public class Grid2DTests
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

    [Fact]
    public void Grid2D_InitializesWithCorrectDimensions()
    {
        // Arrange
        var grid = CreateTestGrid(width: 4, height: 3, Origin.Xy);

        // Act & Assert
        Assert.Equal(4, grid.Width);
        Assert.Equal(3, grid.Height);
    }

    [Fact]
    public void Indexer_ReturnsCorrectValue()
    {
        // Arrange
        var grid = CreateTestGrid(width: 4, height: 3, Origin.Xy);

        // Act & Assert
        Assert.Equal( 0, grid[0, 0]);
        Assert.Equal( 1, grid[1, 0]);
        Assert.Equal( 2, grid[2, 0]);
        Assert.Equal( 3, grid[3, 0]);
        Assert.Equal(11, grid[3, 2]);
    }

    [Fact]
    public void Indexer_SetsValueCorrectly()
    {
        // Arrange
        var grid = CreateTestGrid(width: 4, height: 3, Origin.Xy);
        
        // Act
        grid[2, 1] = 42;

        // Assert
        Assert.Equal(42, grid[2, 1]);
    }

    [Fact]
    public void Contains_ValidatesPositionCorrectly()
    {
        // Arrange
        var grid = CreateTestGrid(width: 4, height: 3, Origin.Xy);

        // Act & Assert
        Assert.True(grid.Contains(new Vec2D(0, 0)));
        Assert.True(grid.Contains(new Vec2D(3, 2)));
        Assert.False(grid.Contains(new Vec2D(-1, 0)));
        Assert.False(grid.Contains(new Vec2D(4, 3)));
    }
    
}