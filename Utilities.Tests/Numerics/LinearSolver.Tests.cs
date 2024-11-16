using Utilities.Numerics;

namespace Utilities.Tests.Numerics;

/// <summary>
///     Tests associated with <see cref="LinearSolver"/>.
/// </summary>
public sealed class LinearSolverTests
{
    [Fact]
    public void Solve_ConsistentSystem_ReturnsExpectedSolution()
    {
        // Arrange
        var coefficients = new double[,]
        {
            { 1, -2,  1,  0 }, // 1x - 2y + 1z =  0
            { 2,  1, -3,  5 }, // 2x + 1y - 3z =  5
            { 4, -7,  1, -1 }  // 4x - 7y + 1x = -1
        };
        var expected = new double[] { 3, 2, 1 }; // Solution: x=3, y=2, z=1

        // Act
        var actual = LinearSolver.Solve(coefficients, epsilon: double.Epsilon);

        // Assert
        Assert.Equal(expected[0], actual[0], precision: 3);
        Assert.Equal(expected[1], actual[1], precision: 3);
        Assert.Equal(expected[2], actual[2], precision: 3);
    }

    [Fact]
    public void Solve_InconsistentSystem_ThrowsException()
    {
        // Arrange
        var coefficients = new double[,]
        {
            { 1,  1, -3, 4 }, // 1x + 1y - 3z = 4
            { 1,  0, -2, 1 }, // 1x + 0y - 2z = 1
            { 0, -1,  1, 2 }  // 0x - 1y + 1z = 2
        };
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => LinearSolver.Solve(coefficients, epsilon: double.Epsilon));
    }
}