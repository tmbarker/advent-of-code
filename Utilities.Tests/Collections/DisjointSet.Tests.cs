using Utilities.Collections;

namespace Utilities.Tests.Collections;

/// <summary>
///     Tests associated with <see cref="DisjointSet{T}"/>.
/// </summary>
public sealed class DisjointSetTests
{
    [Fact]
    public void MakeSet_ShouldAddElement_WhenElementIsNew()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();

        // Act
        var result = disjointSet.MakeSet(element: 1);

        // Assert
        Assert.True(result);
        Assert.True(disjointSet.ContainsElement(1));
        Assert.Equal(1, disjointSet.ElementsCount);
        Assert.Equal(1, disjointSet.PartitionsCount);
    }

    [Fact]
    public void MakeSet_ShouldNotAddElement_WhenElementAlreadyExists()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element:1);

        // Act
        var result = disjointSet.MakeSet(element: 1);

        // Assert
        Assert.False(result);
        Assert.Equal(1, disjointSet.ElementsCount);
        Assert.Equal(1, disjointSet.PartitionsCount);
    }

    [Fact]
    public void Union_ShouldMergeSets_WhenElementsAreInDifferentSets()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element: 1);
        disjointSet.MakeSet(element: 2);

        // Act
        var result = disjointSet.Union(elementA: 1, elementB: 2);

        // Assert
        Assert.True(result);
        Assert.Equal(1, disjointSet.PartitionsCount);
        Assert.Equal(disjointSet.FindSet(element: 1), disjointSet.FindSet(element: 2));
    }

    [Fact]
    public void Union_ShouldNotMergeSets_WhenElementsAreInSameSet()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element: 1);
        disjointSet.MakeSet(element: 2);
        disjointSet.Union(elementA: 1, elementB: 2);

        // Act
        var result = disjointSet.Union(1, 2);

        // Assert
        Assert.False(result);
        Assert.Equal(1, disjointSet.PartitionsCount);
    }

    [Fact]
    public void FindSet_ShouldReturnCorrectRepresentative()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element: 1);
        disjointSet.MakeSet(element: 2);
        disjointSet.MakeSet(element: 3);

        disjointSet.Union(elementA: 1, elementB: 2);

        // Act
        var rep1 = disjointSet.FindSet(element: 1);
        var rep2 = disjointSet.FindSet(element: 2);
        var rep3 = disjointSet.FindSet(element: 3);

        // Assert
        Assert.Equal(rep1, rep2);
        Assert.NotEqual(rep1, rep3);
    }

    [Fact]
    public void ContainsElement_ShouldReturnTrue_IfElementExists()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element: 1);

        // Act
        var result = disjointSet.ContainsElement(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsElement_ShouldReturnFalse_IfElementDoesNotExist()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();

        // Act
        var result = disjointSet.ContainsElement(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Clear_ShouldRemoveAllElementsAndResetPartitions()
    {
        // Arrange
        var disjointSet = new DisjointSet<int>();
        disjointSet.MakeSet(element: 1);
        disjointSet.MakeSet(element: 2);
        disjointSet.Union(elementA: 1, elementB: 2);

        // Act
        disjointSet.Clear();

        // Assert
        Assert.Equal(0, disjointSet.ElementsCount);
        Assert.Equal(0, disjointSet.PartitionsCount);
        Assert.False(disjointSet.ContainsElement(1));
        Assert.False(disjointSet.ContainsElement(2));
    }
}