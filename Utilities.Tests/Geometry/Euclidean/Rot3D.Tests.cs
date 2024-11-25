using Utilities.Geometry.Euclidean;

namespace Utilities.Tests.Geometry.Euclidean;

/// <summary>
///     Tests associated with <see cref="Rot3D"/>.
/// </summary>
public class Rot3DTests
{
    [Fact]
    public void ZeroRotation_IsIdentityQuaternion()
    {
        // Act
        var zeroRotation = Rot3D.Zero;

        // Assert
        Assert.Equal(zeroRotation, Quaternion.Identity);
    }

    [Fact]
    public void P90X_RotatesCorrectly()
    {
        // Arrange
        var vector = new Vec3D(0, 1, 0);   // Along the Y-axis
        var expected = new Vec3D(0, 0, 1); // Rotated to Z-axis

        // Act
        var result = Rot3D.P90X.Transform(vector);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void N90Y_RotatesCorrectly()
    {
        // Arrange
        var vector = new Vec3D(1, 0, 0); // Along the X-axis
        var expected = new Vec3D(0, 0, 1); // Rotated to Z-axis

        // Act
        var result = Rot3D.N90Y.Transform(vector);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void P90Z_RotatesCorrectly()
    {
        // Arrange
        var vector = new Vec3D(1, 0, 0);   // Along the X-axis
        var expected = new Vec3D(0, 1, 0); // Rotated to Y-axis

        // Act
        var result = Rot3D.P90Z.Transform(vector);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void InvalidAxis_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const Axis invalidAxis = (Axis)100;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Rot3D.FromAxisAngle(invalidAxis, angleDeg: 90));
    }

    [Theory]
    [InlineData(Axis.X, 0)]
    [InlineData(Axis.Y, 0)]
    [InlineData(Axis.Z, 0)]
    public void FromAxisAngle_ZeroAngle_IsIdentityQuaternion(Axis axis, int angle)
    {
        // Act
        var result = Rot3D.FromAxisAngle(axis, angle);

        // Assert
        Assert.Equal(result, Quaternion.Identity);
    }

    [Theory]
    [InlineData(Axis.X, 180, 0, 1, 0, 0)] // Rotation 180° around X-axis
    [InlineData(Axis.Y, 180, 0, 0, 1, 0)] // Rotation 180° around Y-axis
    [InlineData(Axis.Z, 180, 0, 0, 0, 1)] // Rotation 180° around Z-axis
    public void FromAxisAngle_180Degrees_IsCorrectQuaternion(Axis axis, int angle, 
        double expectedW, double expectedX, double expectedY, double expectedZ)
    {
        // Act
        var result = Rot3D.FromAxisAngle(axis, angle);

        // Assert
        Assert.Equal(expectedW, result.W, precision: 10);
        Assert.Equal(expectedX, result.X, precision: 10);
        Assert.Equal(expectedY, result.Y, precision: 10);
        Assert.Equal(expectedZ, result.Z, precision: 10);
    }
}