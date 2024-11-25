namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A helper class providing integral multiples of 90 degree rotations about the <see cref="Axis.X" />,
///     <see cref="Axis.Y" />, and <see cref="Axis.Z" /> axes.
/// </summary>
public static class Rot3D
{
    public static readonly Quaternion Zero  = Quaternion.Identity;
    public static readonly Quaternion N90X  = FromAxisAngle(axis: Axis.X, angleDeg: Degrees.N90);
    public static readonly Quaternion P90X  = FromAxisAngle(axis: Axis.X, angleDeg: Degrees.P90);
    public static readonly Quaternion P180X = FromAxisAngle(axis: Axis.X, angleDeg: Degrees.P180);
    public static readonly Quaternion N90Y  = FromAxisAngle(axis: Axis.Y, angleDeg: Degrees.N90);
    public static readonly Quaternion P90Y  = FromAxisAngle(axis: Axis.Y, angleDeg: Degrees.P90);
    public static readonly Quaternion P180Y = FromAxisAngle(axis: Axis.Y, angleDeg: Degrees.P180);
    public static readonly Quaternion N90Z  = FromAxisAngle(axis: Axis.Z, angleDeg: Degrees.N90);
    public static readonly Quaternion P90Z  = FromAxisAngle(axis: Axis.Z, angleDeg: Degrees.P90);
    public static readonly Quaternion P180Z = FromAxisAngle(axis: Axis.Z, angleDeg: Degrees.P180);

    /// <summary>
    ///     Build a <see cref="Quaternion"/> representing a 3D rotation using the specified <see cref="Axis"/>
    ///     and amount.
    /// </summary>
    /// <param name="axis">The axis of rotation</param>
    /// <param name="angleDeg">The angle of rotation, in degrees</param>
    /// <returns>A <see cref="Quaternion"/> representing the rotation</returns>
    /// <exception cref="ArgumentOutOfRangeException">An invalid <see cref="Axis"/> was specified</exception>
    public static Quaternion FromAxisAngle(Axis axis, int angleDeg)
    {
        return FromAxisAngle(axis, angleRad: angleDeg * Math.PI / Degrees.P180);
    }
    
    private static Quaternion FromAxisAngle(Axis axis, double angleRad)
    {
        return axis switch
        {
            Axis.X => FromAxisAngle(ux: 1, uy: 0, uz: 0, angleRad),
            Axis.Y => FromAxisAngle(ux: 0, uy: 1, uz: 0, angleRad),
            Axis.Z => FromAxisAngle(ux: 0, uy: 0, uz: 1, angleRad),
            _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, $"Invalid axis [{axis}]")
        };
    }
    
    private static Quaternion FromAxisAngle(double ux, double uy, double uz, double angleRad)
    {
        var halfAngle = angleRad / 2;
        var sinHalfAngle = Math.Sin(halfAngle);
        
        return new Quaternion(
            W: Math.Cos(halfAngle),
            X: sinHalfAngle * ux,
            Y: sinHalfAngle * uy,
            Z: sinHalfAngle * uz
        );
    }
}