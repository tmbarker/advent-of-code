using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Tests.Extensions;

/// <summary>
///     Tests associated with <see cref="RegexExtensions"/>.
/// </summary>
public partial class RegexExtensionsTests
{
    [GeneratedRegex(pattern:@"(?<number>\d+)")]
    private static partial Regex TestRegex();
    
    private const string TestInput = "123 and 456 and 789";
    private const string TestGroupName = "number";
    
    private static readonly int[] TestCaptureIntegers = [123, 456, 789];
    private static readonly string[] TestCaptureStrings = ["123", "456", "789"];
    
    [Theory]
    [InlineData("123", 123, true)]
    [InlineData("abc", 0, false)]
    [InlineData("", 0, false)]
    public void ParseIntOrDefault_ReturnsCorrectValue(string input, int expectedValue, bool expectedSuccess)
    {
        // Arrange
        var regex = TestRegex();
        var match = regex.Match(input);

        // Act
        var value = match.ParseIntOrDefault();

        // Assert
        Assert.Equal(expectedSuccess, match.Success);
        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void ParseInts_FromGroup_ReturnsAllIntegers()
    {
        // Arrange
        var regex = TestRegex();
        var matches = regex.Matches(TestInput);

        // Act
        var result = matches
            .SelectMany(match => match.Groups[1].ParseInts())
            .ToArray();

        // Assert
        Assert.Equal(TestCaptureIntegers, result);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("456", 456)]
    public void ParseInt_FromCapture_ReturnsCorrectValue(string input, int expected)
    {
        // Arrange
        var regex = TestRegex();
        var match = regex.Match(input);
        var capture = match.Groups[1].Captures[0];

        // Act
        var result = capture.ParseInt();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123456789123456789", 123456789123456789)]
    public void ParseLong_FromCapture_ReturnsCorrectValue(string input, long expected)
    {
        // Arrange
        var regex = TestRegex();
        var match = regex.Match(input);
        var capture = match.Groups[1].Captures[0];

        // Act
        var result = capture.ParseLong();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SelectValues_ReturnsAllMatchValues()
    {
        // Arrange
        var regex = TestRegex();
        var matches = regex.Matches(TestInput);

        // Act
        var result = matches.SelectValues();

        // Assert
        Assert.Equal(TestCaptureStrings, result);
    }

    [Fact]
    public void SelectCaptures_ByGroupIndex_ReturnsAllCaptureValues()
    {
        // Arrange
        var regex = TestRegex();
        var matches = regex.Matches(TestInput);

        // Act
        var result = matches.SelectCaptures(group: 1);

        // Assert
        Assert.Equal(TestCaptureStrings, result);
    }

    [Fact]
    public void SelectCaptures_ByGroupName_ReturnsAllCaptureValues()
    {
        // Arrange
        var regex = TestRegex();
        var matches = regex.Matches(TestInput);

        // Act
        var result = matches.SelectCaptures(group: TestGroupName);

        // Assert
        Assert.Equal(TestCaptureStrings, result);
    }
}