using Utilities.Extensions;

namespace Utilities.DataStructures.Cartesian;

/// <summary>
/// A readonly value type which provides integral 90 degree rotations over <see cref="Vector3D"/> instances 
/// </summary>
public readonly struct Rotation3D : IEquatable<Rotation3D>
{
    private const string ThetaOutOfRangeError = "Theta must be an integral multiple of 90 degrees";
    private const int DegreesPerRotation = 360;
    private const int NinetyDegrees = DegreesPerRotation / 4;

    public static readonly Rotation3D Zero = new(RotationAxis.X, 0);
    public static readonly Rotation3D Negative90X = new(RotationAxis.X, -NinetyDegrees);
    public static readonly Rotation3D Positive90X = new(RotationAxis.X, NinetyDegrees);
    public static readonly Rotation3D Positive180X = new(RotationAxis.X, 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Y = new(RotationAxis.Y, -NinetyDegrees);
    public static readonly Rotation3D Positive90Y = new(RotationAxis.Y, NinetyDegrees);
    public static readonly Rotation3D Positive180Y = new(RotationAxis.Y, 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Z = new(RotationAxis.Z, -NinetyDegrees);
    public static readonly Rotation3D Positive90Z = new(RotationAxis.Z, NinetyDegrees);
    public static readonly Rotation3D Positive180Z = new(RotationAxis.Z, 2 * NinetyDegrees);

    private RotationAxis Axis { get; }
    private int ThetaDeg { get; }
    private double ThetaRad { get; }

    private Rotation3D(RotationAxis axis, int thetaDeg)
    {
        if (thetaDeg.Modulo(NinetyDegrees) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thetaDeg), thetaDeg, ThetaOutOfRangeError);
        }

        Axis = axis;
        ThetaDeg = thetaDeg.Modulo(DegreesPerRotation);
        ThetaRad = thetaDeg * Math.PI * 2 / DegreesPerRotation;
    }

    public static Vector3D operator *(Rotation3D r, Vector3D v)
    {
        return r.Axis switch
        {
            RotationAxis.X => RotateAboutX(r, v),
            RotationAxis.Y => RotateAboutY(r, v),
            RotationAxis.Z => RotateAboutZ(r, v),
            _ => throw new InvalidOperationException($"Invalid axis of rotation [{r.Axis}]"),
        };
    }

    private static Vector3D RotateAboutX(Rotation3D r, Vector3D v)
    {
        var y = v.Y * Math.Cos(r.ThetaRad) - v.Z * Math.Sin(r.ThetaRad);
        var z = v.Y * Math.Sin(r.ThetaRad) + v.Z * Math.Cos(r.ThetaRad);
        return new Vector3D(v.X, (int)Math.Round(y), (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutY(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r.ThetaRad) + v.Z * Math.Sin(r.ThetaRad);
        var z = v.Z * Math.Cos(r.ThetaRad) - v.X * Math.Sin(r.ThetaRad);
        return new Vector3D((int)Math.Round(x), v.Y, (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutZ(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r.ThetaRad) - v.Y * Math.Sin(r.ThetaRad);
        var y = v.X * Math.Sin(r.ThetaRad) + v.Y * Math.Cos(r.ThetaRad);
        return new Vector3D((int)Math.Round(x), (int)Math.Round(y), v.Z);
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

    private enum RotationAxis
    {
        X,
        Y,
        Z,
    }
}