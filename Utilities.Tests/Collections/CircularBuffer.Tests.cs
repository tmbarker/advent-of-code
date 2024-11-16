using Utilities.Collections;

namespace Utilities.Tests.Collections;

/// <summary>
///     Tests associated with <see cref="CircularBuffer{T}"/>. 
/// </summary>
public sealed class CircularBufferTests
{
    [Fact]
    public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CircularBuffer<int>(capacity: -1));
    }

    [Fact]
    public void Constructor_WithItemsExceedingCapacity_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CircularBuffer<int>(capacity: 2, [1, 2, 3]));
    }

    [Fact]
    public void Capacity_Get_ReturnsCorrectValue()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<int>(capacity: 2);
        
        // Act & Assert
        Assert.Equal(2, circularBuffer.Capacity);
    }
    
    [Fact]
    public void Enqueue_AddsItemsToBuffer()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 2);
        
        // Act
        buffer.Enqueue(1);
        buffer.Enqueue(2);

        // Assert
        Assert.True(buffer.IsFull);
        Assert.Equal(2, buffer.Count);
        Assert.Equal(1, buffer.PeekHead());
        Assert.Equal(2, buffer.PeekTail());
    }

    [Fact]
    public void Enqueue_OverwritesWhenFull()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3);
        
        // Act
        buffer.Enqueue(1);
        buffer.Enqueue(2);
        buffer.Enqueue(3);
        buffer.Enqueue(4); // Overwrites 1

        // Assert
        Assert.True(buffer.IsFull);
        Assert.Equal(3, buffer.Count);
        Assert.Equal(2, buffer.PeekHead());
        Assert.Equal(4, buffer.PeekTail());
    }

    [Fact]
    public void Dequeue_RemovesItemsFromBuffer()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3, [1, 2, 3]);
        
        // Act
        var item = buffer.Dequeue();

        // Assert
        Assert.Equal(1, item);
        Assert.Equal(2, buffer.Count);
        Assert.Equal(2, buffer.PeekHead());
    }

    [Fact]
    public void PeekHead_ThrowsWhenBufferIsEmpty()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => buffer.PeekHead());
    }

    [Fact]
    public void PeekTail_ThrowsWhenBufferIsEmpty()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => buffer.PeekTail());
    }

    [Fact]
    public void Clear_EmptiesBuffer()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3, [1, 2, 3]);
        
        // Act
        buffer.Clear();

        // Assert
        Assert.True(buffer.IsEmpty);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3, [1, 2, 3]);
        
        // Act & Assert
        Assert.Equal(1, buffer[0]);
        Assert.Equal(2, buffer[1]);
        Assert.Equal(3, buffer[2]);
    }

    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        // Arrange & Act
        var buffer = new CircularBuffer<int>(capacity: 3, [1, 2, 3])
        {
            [1] = 4
        };

        // Assert
        Assert.Equal(4, buffer[1]);
    }

    [Fact]
    public void Indexer_ThrowsForInvalidIndex()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3, [1, 2, 3]);
        
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => buffer[3]);
        Assert.Throws<IndexOutOfRangeException>(() => buffer[-1]);
    }

    [Fact]
    public void Enumerate_BufferReturnsItemsInCorrectOrder()
    {
        // Arrange
        var array = new [] { 1, 2, 3 };
        var buffer = new CircularBuffer<int>(capacity: 3, items: array);

        // Act & Assert
        Assert.True(array.SequenceEqual(buffer));
    }

    [Fact]
    public void ToArraySegments_ReturnsCorrectSegmentsForWrappedBuffer()
    {
        // Arrange
        var buffer = new CircularBuffer<int>(capacity: 3);
        var expected = new [] { 2, 3, 4 };
        
        // Act
        buffer.Enqueue(1);
        buffer.Enqueue(2);
        buffer.Enqueue(3);
        buffer.Dequeue();  // Remove 1
        buffer.Enqueue(4); // Buffer is now wrapped: [4,2,3]
        
        // Assert
        Assert.True(buffer.SequenceEqual(expected));
    }
}