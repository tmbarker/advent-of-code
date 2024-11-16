using Utilities.Numerics;

namespace Utilities.Tests.Numerics;

/// <summary>
///     Tests associated with <see cref="Range{T}"/>.
/// </summary>
public sealed class RangeTests
{
    [Fact]
    public void Overlap_WhenRangesDoNotOverlap_ShouldReturnFalse()
    {
        // Arrange
        var range1 = new Range<int>(1, 5);
        var range2 = new Range<int>(6, 10);
    
        // Act
        var result = Range<int>.Overlap(range1, range2, overlap: out _);
    
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Overlap_WhenRangesAreExactlyEqual_ShouldReturnSameRange()
    {
        // Arrange
        var range1 = new Range<int>(1, 5);
        var range2 = new Range<int>(1, 5);
    
        // Act
        var result = Range<int>.Overlap(range1, range2, out var overlap);
    
        // Assert
        Assert.True(result);
        Assert.Equal(range1, overlap);
        Assert.Equal(range2, overlap);
    }
    
    [Fact]
    public void Overlap_WhenRangesPartiallyOverlap_ShouldReturnOverlapRange()
    {
        // Arrange
        var range1 = new Range<int>(1, 5);
        var range2 = new Range<int>(4, 10);
        var expected = new Range<int>(4, 5);
    
        // Act
        var result = Range<int>.Overlap(range1, range2, out var actual);
    
        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Overlap_WhenRangeIsContainedWithinAnother_ShouldReturnContainingRange()
    {
        // Arrange
        var outer = new Range<int>(1, 10);
        var inner = new Range<int>(3, 5);
    
        // Act
        var result = Range<int>.Overlap(outer, inner, out var overlap);
    
        // Assert
        Assert.True(result);
        Assert.Equal(inner, overlap);
    }

    [Fact]
    public void Contains_WhenRangeContainsValue_ShouldReturnTrue()
    {
        // Arrange
        var range = new Range<int>(1, 5);
        
        // Act
        var result = range.Contains(3);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void Contains_WhenRangeBeginsOrEndsWithValue_ShouldReturnTrue()
    {
        // Arrange
        var range = new Range<int>(1, 5);
        
        // Act
        var result1 = range.Contains(1);
        var result2 = range.Contains(5);
        
        // Assert
        Assert.True(result1);
        Assert.True(result2);
    }

    [Fact]
    public void Contains_WhenRangeDoesNotContainValue_ShouldReturnFalse()
    {
        // Arrange
        var range = new Range<int>(1, 5);
        
        // Act
        var result = range.Contains(0);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Contains_WhenRangeContainsRange_ShouldReturnTrue()
    {
        // Arrange
        var range1 = new Range<int>(1, 5);
        var range2 = new Range<int>(2, 4);
        
        // Act
        var result = range1.Contains(range2);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WhenRangeDoesNotContainRange_ShouldReturnFalse()
    {
        // Arrange
        var range1 = new Range<int>(1, 5);
        var range2 = new Range<int>(6, 10);
        
        // Act
        var result = range1.Contains(range2);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Single_Length_ShouldBeOne()
    {
        // Arrange
        var range = Range<int>.Single(value: 5);
        
        // Act
        var length = range.Length;
        
        // Assert
        Assert.Equal(1, length);
    }

    [Fact]
    public void Length_ShouldReturnExpectedValue()
    {
        // Arrange
        var range = new Range<int>(1, 5);
        
        // Act
        var length = range.Length;
        
        // Assert
        Assert.Equal(5, length);
    }
    
    public static TheoryData<string, Range<int>> ParseTestData => new()
    {
        { "1 - 5",   new Range<int>( 1,  5) },
        { "1-5",     new Range<int>( 1,  5) },
        { "1 , 5",   new Range<int>( 1,  5) },
        { "1,5",     new Range<int>( 1,  5) },
        { "-5 - -1", new Range<int>(-5, -1) },
        
    };
    
    [Theory]
    [MemberData(nameof(ParseTestData))]
    public void Parse_ShouldReturnExpectedRange(string str, Range<int> expected)
    {
        // Act
        var actual = Range<int>.Parse(str);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}