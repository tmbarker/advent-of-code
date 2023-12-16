using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly value type which provides integral 90 degree rotations over <see cref="Vector3D"/> instances 
/// </summary>
public readonly struct Rotation3D : IEquatable<Rotation3D>
{
    public static readonly Rotation3D Zero         = new(axis: Axis.X, thetaDeg: Degrees.Zero);
    public static readonly Rotation3D Negative90X  = new(axis: Axis.X, thetaDeg: Degrees.N90);
    public static readonly Rotation3D Positive90X  = new(axis: Axis.X, thetaDeg: Degrees.P90);
    public static readonly Rotation3D Positive180X = new(axis: Axis.X, thetaDeg: Degrees.P180);
    public static readonly Rotation3D Negative90Y  = new(axis: Axis.Y, thetaDeg: Degrees.N90);
    public static readonly Rotation3D Positive90Y  = new(axis: Axis.Y, thetaDeg: Degrees.P90);
    public static readonly Rotation3D Positive180Y = new(axis: Axis.Y, thetaDeg: Degrees.P180);
    public static readonly Rotation3D Negative90Z  = new(axis: Axis.Z, thetaDeg: Degrees.N90);
    public static readonly Rotation3D Positive90Z  = new(axis: Axis.Z, thetaDeg: Degrees.P90);
    public static readonly Rotation3D Positive180Z = new(axis: Axis.Z, thetaDeg: Degrees.P180);

    public Rotation3D(Axis axis, int thetaDeg)
    {
        if (axis is not (Axis.X or Axis.Y or Axis.Z))
        {
            throw ThrowHelper.InvalidAxis(axis);
        }
        
        if (thetaDeg.Modulo(Degrees.P90) != 0)
        {
            throw ThrowHelper.InvalidTheta(thetaDeg);
        }

        Axis = axis;
        ThetaDeg = thetaDeg.Modulo(Degrees.P360);
        ThetaRad = DegToRad(thetaDeg);
    }

    public Axis Axis { get; }
    public int ThetaDeg { get; }
    private double ThetaRad { get; }

    public static Vector3D operator *(Rotation3D r, Vector3D v)
    {
        return r.Axis switch
        {
            Axis.X => RotateAboutX(r, v),
            Axis.Y => RotateAboutY(r, v),
            Axis.Z => RotateAboutZ(r, v),
            _ => throw ThrowHelper.InvalidAxis(r.Axis)
        };
    }

    public static IEnumerable<Rotation3D> RotationsAroundAxis(Axis axis)
    {
        if (axis is not (Axis.X or Axis.Y or Axis.Z))
        {
            throw ThrowHelper.InvalidAxis(axis);
        }
        
        for (var i = 0; i < Degrees.P360 / Degrees.P90; i++)
        {
            yield return new Rotation3D(axis, thetaDeg: i * Degrees.P90);
        }
    }

    private static double DegToRad(int deg) => deg * Math.PI * 2 / Degrees.P360;
    
    private static Vector3D RotateAboutX(Rotation3D r, Vector3D v)
    {
        var y = v.Y * Math.Cos(r.ThetaRad) - v.Z * Math.Sin(r.ThetaRad);
        var z = v.Y * Math.Sin(r.ThetaRad) + v.Z * Math.Cos(r.ThetaRad);
        return new Vector3D(v.X, y: (int)Math.Round(y), z: (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutY(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r.ThetaRad) + v.Z * Math.Sin(r.ThetaRad);
        var z = v.Z * Math.Cos(r.ThetaRad) - v.X * Math.Sin(r.ThetaRad);
        return new Vector3D(x: (int)Math.Round(x), v.Y, z: (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutZ(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r.ThetaRad) - v.Y * Math.Sin(r.ThetaRad);
        var y = v.X * Math.Sin(r.ThetaRad) + v.Y * Math.Cos(r.ThetaRad);
        return new Vector3D(x: (int)Math.Round(x), y: (int)Math.Round(y), v.Z);
    }
    
    public static bool operator ==(Rotation3D lhs, Rotation3D rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Rotation3D lhs, Rotation3D rhs)
    {
        return !lhs.Equals(rhs);
    }

    public bool Equals(Rotation3D other)
    {
        return ThetaDeg == other.ThetaDeg && Axis == other.Axis;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rotation3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Axis, ThetaDeg);
    }

    public override string ToString()
    {
        return $"R({Axis}): {ThetaDeg}°";
    }
    
    /// <summary>
    /// Internal throw helper for <see cref="Rotation3D"/>
    /// </summary>
    private static class ThrowHelper
    {
        private const string ThetaOutOfRangeError = "Theta must be an integer multiple of 90 degrees";
        private const string AxisOutOfRangeError = "Axis must be a 3D Eucliedean axis (X, Y, or Z)";

        internal static Exception InvalidTheta(int theta)
        {
            throw new ArgumentOutOfRangeException(nameof(theta), theta, message: ThetaOutOfRangeError);
        }
        
        internal static Exception InvalidAxis(Axis axis)
        {
            throw new ArgumentOutOfRangeException(nameof(axis), axis, message: AxisOutOfRangeError);
        }
    }
}