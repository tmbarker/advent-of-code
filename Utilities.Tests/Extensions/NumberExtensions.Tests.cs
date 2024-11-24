using Utilities.Extensions;

namespace Utilities.Tests.Extensions;

/// <summary>
///     Tests associated with <see cref="NumberExtensions"/>.
/// </summary>
public class NumberExtensionsTests
{
    [Theory]
    [InlineData(10, 3, 1)]
    [InlineData(-10, 3, 2)]
    [InlineData(10, -3, -2)]
    [InlineData(-10, -3, -1)]
    [InlineData(10, 5, 0)]
    public void Modulo_ReturnsCorrectResult(int a, int modulus, int expected)
    {
        // Act
        var result = a.Modulo(modulus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Modulo_ZeroModulus_ThrowsDivideByZeroException()
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => 10.Modulo(0));
    }
}