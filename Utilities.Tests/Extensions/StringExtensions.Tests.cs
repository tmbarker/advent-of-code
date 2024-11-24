using Utilities.Extensions;

namespace Utilities.Tests.Extensions;

/// <summary>
///     Tests associated with <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionsTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    [InlineData("0", 0)]
    [InlineData("  789  ", 789)]
    public void ParseNumber_ReturnsCorrectResult(string input, int expected)
    {
        // Act
        var result = input.ParseInt();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    public void ParseNumber_InvalidInput_ThrowsException(string input)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => input.ParseInt());
    }

    [Theory]
    [InlineData("123 abc -456 789", new[] { 123, -456, 789 })]
    [InlineData("no numbers here", new int[0])]
    [InlineData("  1 2 3  ", new[] { 1, 2, 3 })]
    public void ParseNumbers_ReturnsCorrectArray(string input, int[] expected)
    {
        // Act
        var result = input.ParseInts();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123abc", 123)]
    public void ParseNumber_HandlesStringsWithMultipleDigits(string input, int expected)
    {
        // Act
        var result = input.ParseInt();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData('5', 5)]
    [InlineData('0', 0)]
    [InlineData('9', 9)]
    public void AsDigit_ValidDigit_ReturnsCorrectInteger(char input, int expected)
    {
        // Act
        var result = input.AsDigit();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(" a b  c   ", "abc")]
    [InlineData("  123  456 ", "123456")]
    [InlineData("NoSpaces", "NoSpaces")]
    [InlineData("", "")]
    public void RemoveWhitespace_RemovesAllWhitespace(string input, string expected)
    {
        // Act
        var result = input.RemoveWhitespace();

        // Assert
        Assert.Equal(expected, result);
    }
}