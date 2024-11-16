using Utilities.Collections;

namespace Utilities.Tests.Collections;

/// <summary>
///     Tests associated with <see cref="CircularLinkedList{T}"/>.
/// </summary>
public sealed class CircularLinkedListTests
{
    [Fact]
    public void AddFirst_ShouldAddNodeAsHead()
    {
        // Arrange
        var list = new CircularLinkedList<int>();

        // Act
        list.AddFirst(10);

        // Assert
        Assert.NotNull(list.Head);
        Assert.Equal(1, list.Count);
        Assert.Equal(10, list.Head.Value);
        Assert.Same(list.Head, list.Head.Next);
        Assert.Same(list.Head, list.Head.Prev);
    }

    [Fact]
    public void AddLast_ShouldAddNodeAsTail()
    {
        // Arrange
        var list = new CircularLinkedList<int>();

        // Act
        list.AddLast(20);

        // Assert
        Assert.NotNull(list.Tail);
        Assert.Equal(1, list.Count);
        Assert.Equal(20, list.Tail.Value);
        Assert.Same(list.Tail, list.Tail.Next);
        Assert.Same(list.Tail, list.Tail.Prev);
    }

    [Fact]
    public void RemoveHead_ShouldRemoveHeadNode()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddFirst(10);
        list.AddLast(20);

        // Act
        list.RemoveHead();

        // Assert
        Assert.NotNull(list.Head);
        Assert.Equal(1, list.Count);
        Assert.Equal(20, list.Head.Value);
    }

    [Fact]
    public void Reverse_ShouldReverseList()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddFirst(10);
        list.AddLast(20);
        list.AddLast(30);

        // Act
        list.Reverse(preserveHead: false);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.NotNull(list.Head);
        Assert.Equal(30, list.Head.Value);
        Assert.Equal(20, list.Head.Next!.Value);
        Assert.Equal(10, list.Head.Next.Next!.Value);
    }

    [Fact]
    public void Clear_ShouldEmptyList()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddFirst(10);
        list.AddLast(20);

        // Act
        list.Clear();

        // Assert
        Assert.Equal(0, list.Count);
        Assert.Null(list.Head);
        Assert.Null(list.Tail);
    }

    [Fact]
    public void GetNode_ShouldReturnCorrectNode()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddLast(10);
        list.AddLast(20);
        list.AddLast(30);

        // Act
        var node = list.GetNode(index: 1);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(20, node.Value);
    }

    [Fact]
    public void FindNode_ShouldReturnNodeAndIndex()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddLast(10);
        list.AddLast(20);
        list.AddLast(30);

        // Act
        var node = list.FindNode(20, out var index);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(1, index);
        Assert.Equal(20, node.Value);
    }

    [Fact]
    public void ReverseRange_ShouldReverseSubList()
    {
        // Arrange
        var list = new CircularLinkedList<int>();
        list.AddLast(10);
        list.AddLast(20);
        list.AddLast(30);
        list.AddLast(40);

        var startNode = list.GetNode(1);

        // Act
        list.ReverseRange(startNode, count: 2, preserveHead: true);

        // Assert
        Assert.Equal(10, list.Head!.Value);
        Assert.Equal(30, list.Head.Next!.Value);
        Assert.Equal(20, list.Head.Next.Next!.Value);
        Assert.Equal(40, list.Head.Next.Next.Next!.Value);
    }
}