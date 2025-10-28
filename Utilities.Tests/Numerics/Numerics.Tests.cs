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
    
    public static TheoryData<int, int, int, int> ModAddTestData => new()
    {
        {  7,  3, 10,  0 },
        {  5,  8, 12,  1 },
        { 15, 17,  7,  4 },
        { 23, 18, 13,  2 },
        {  9, 14, 20,  3 }
    };
    
    [Theory]
    [MemberData(nameof(ModAddTestData))]
    public void ModAdd_ShouldReturnExpectedValue(int a, int b, int m, int expected)
    {
        // Act
        var actual = Utilities.Numerics.Numerics.ModAdd(a, b, m);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    public static TheoryData<int, int, int, int> ModMultiplyTestData => new()
    {
        {  3,  4, 11,  1 },
        {  7,  8, 13,  4 },
        { 12, 15, 17, 10 },
        {  5,  9, 23, 22 },
        { 14, 11, 19,  2 }
    };
    
    [Theory]
    [MemberData(nameof(ModMultiplyTestData))]
    public void ModMultiply_ShouldReturnExpectedValue(int a, int b, int m, int expected)
    {
        // Act
        var actual = Utilities.Numerics.Numerics.ModMultiply(a, b, m);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    public static TheoryData<int, int, int> ModInverseTestData => new()
    {
        {  3, 11,  4 },
        {  7, 13,  2 },
        {  5, 17,  7 },
        {  9, 23, 18 },
        { 15, 19, 14 }
    };
    
    [Theory]
    [MemberData(nameof(ModInverseTestData))]
    public void ModInverse_ShouldReturnExpectedValue(int a, int m, int expected)
    {
        // Act
        var actual = Utilities.Numerics.Numerics.ModInverse(a, m);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ModInverse_ShouldThrowWhenInverseDoesNotExist()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            Utilities.Numerics.Numerics.ModInverse(6, 9));
    }
}