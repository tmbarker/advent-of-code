namespace Utilities.Tests.Numerics;

/// <summary>
///     Tests associated with <see cref="Numerics"/>.
/// </summary>
public sealed class NumericsTests
{
    public static TheoryData<int, int, int> GcdTestData => new()
    {
        { 12, 18, 6 },
        { 14, 35, 7 },
        {  9, 28, 1 },
        { 25, 10, 5 },
        { 13, 17, 1 }
    };
    
    [Theory]
    [MemberData(nameof(GcdTestData))]
    public void Gcd_ShouldReturnExpectedValue(int a, int b, int expected)
    {
        // Act
        var actual = Utilities.Numerics.Numerics.Gcd(a, b);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    public static TheoryData<int, int, int> LcmTestData => new()
    {
        {  4,  5, 20  },
        {  6,  8, 24  },
        { 12, 15, 60  },
        {  9, 14, 126 },
        { 21, 14, 42  }
    };
    
    [Theory]
    [MemberData(nameof(LcmTestData))]
    public void Lcm_ShouldReturnExpectedValue(int a, int b, int expected)
    {
        // Act
        var actual = Utilities.Numerics.Numerics.Lcm(a, b);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}