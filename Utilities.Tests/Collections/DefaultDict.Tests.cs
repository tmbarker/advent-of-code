using Utilities.Collections;

namespace Utilities.Tests.Collections;

/// <summary>
///     Tests associated with <see cref="DefaultDict{TKey,TValue}"/>.
/// </summary>
public sealed class DefaultDictTests
{
    [Fact]
    public void GetValue_ReturnsDefaultForMissingKey()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 42);

        // Act
        var value = defaultDict["missing"];

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void GetValue_UsesSelectorForDefault()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(key => key.Length);

        // Act
        var value = defaultDict["abc"];

        // Assert
        Assert.Equal(3, value);
    }

    [Fact]
    public void AddValue_StoresAndRetrievesValue()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0) { { "key", 100 } };

        // Act
        var value = defaultDict["key"];

        // Assert
        Assert.Equal(100, value);
    }

    [Fact]
    public void RemoveValue_RemovesKeyValuePair()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0) { { "key", 100 } };

        // Act
        var removed = defaultDict.Remove("key");

        // Assert
        Assert.True(removed);
        Assert.Equal(0, defaultDict["key"]);
    }

    [Fact]
    public void ContainsKey_ReturnsTrueIfKeyExists()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0) { { "key", 100 } };

        // Act
        var exists = defaultDict.ContainsKey("key");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void ContainsKey_ReturnsFalseIfKeyDoesNotExist()
    {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0);

        // Act
        var exists = defaultDict.ContainsKey("missing");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public void Clear_RemovesAllEntries()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0)
        {
            { "key1", 100 },
            { "key2", 200 }
        };

        // Act
        defaultDict.Clear();

        // Assert
        Assert.Empty(defaultDict);
    }

    [Fact]
    public void Count_ReturnsNumberOfItems()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0)
        {
            { "key1", 100 },
            { "key2", 200 }
        };

        // Act
        var count = defaultDict.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void CopyTo_CopiesContentsToArray()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0)
        {
            { "key1", 100 },
            { "key2", 200 }
        };
        var array = new KeyValuePair<string, int>[2];

        // Act
        defaultDict.CopyTo(array, arrayIndex: 0);

        // Assert
        Assert.Contains(new KeyValuePair<string, int>("key1", 100), array);
        Assert.Contains(new KeyValuePair<string, int>("key2", 200), array);
    }

    [Fact]
    public void Enumerator_IteratesOverEntries()
    {
        // Arrange
        var defaultDict = new DefaultDict<string, int>(defaultValue: 0)
        {
            { "key1", 100 },
            { "key2", 200 }
        };

        // Act
        var entries = new List<KeyValuePair<string, int>>();
        foreach (var entry in defaultDict)
        {
            entries.Add(entry);
        }

        // Assert
        Assert.Equal(2, entries.Count);
        Assert.Contains(new KeyValuePair<string, int>("key1", 100), entries);
        Assert.Contains(new KeyValuePair<string, int>("key2", 200), entries);
    }
}