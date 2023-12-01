using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly value type which provides integral 90 degree rotations over <see cref="Vector3D"/> instances 
/// </summary>
public readonly struct Rotation3D : IEquatable<Rotation3D>
{
    private const string ThetaOutOfRangeError = "Theta must be an integral multiple of 90 degrees";
    private const string AxisOutOfRangeError = "Axis must be a 3D spatial axis (X, Y, or Z)";
    private const int DegreesPerRotation = 360;
    private const int NinetyDegrees = DegreesPerRotation / 4;

    private readonly double _thetaRad;

    public static readonly Rotation3D Zero         = new(axis: Axis.X, thetaDeg: 0);
    public static readonly Rotation3D Negative90X  = new(axis: Axis.X, thetaDeg: -NinetyDegrees);
    public static readonly Rotation3D Positive90X  = new(axis: Axis.X, thetaDeg: NinetyDegrees);
    public static readonly Rotation3D Positive180X = new(axis: Axis.X, thetaDeg: 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Y  = new(axis: Axis.Y, thetaDeg: -NinetyDegrees);
    public static readonly Rotation3D Positive90Y  = new(axis: Axis.Y, thetaDeg: NinetyDegrees);
    public static readonly Rotation3D Positive180Y = new(axis: Axis.Y, thetaDeg: 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Z  = new(axis: Axis.Z, thetaDeg: -NinetyDegrees);
    public static readonly Rotation3D Positive90Z  = new(axis: Axis.Z, thetaDeg: NinetyDegrees);
    public static readonly Rotation3D Positive180Z = new(axis: Axis.Z, thetaDeg: 2 * NinetyDegrees);

    public Rotation3D(Axis axis, int thetaDeg)
    {
        if (axis == Axis.W)
        {
            throw new ArgumentOutOfRangeException(nameof(axis), axis, AxisOutOfRangeError);
        }
        
        if (thetaDeg.Modulo(NinetyDegrees) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thetaDeg), thetaDeg, ThetaOutOfRangeError);
        }

        Axis = axis;
        ThetaDeg = thetaDeg.Modulo(DegreesPerRotation);
        _thetaRad = thetaDeg * Math.PI * 2 / DegreesPerRotation;
    }

    private int ThetaDeg { get; }
    public Axis Axis { get; }

    public static Vector3D operator *(Rotation3D r, Vector3D v)
    {
        switch (r.Axis)
        {
            case Axis.X:
                return RotateAboutX(r, v);
            case Axis.Y:
                return RotateAboutY(r, v);
            case Axis.Z:
                return RotateAboutZ(r, v);
            case Axis.W:
            default:
                throw new InvalidOperationException($"Invalid axis of rotation [{r.Axis}]");
        }
    }
    
    public static Vector4D operator *(Rotation3D r, Vector4D v)
    {
        var spatial = r * (Vector3D)v;
        return new Vector4D(
            x: spatial.X,
            y: spatial.Y,
            z: spatial.Z,
            w: v.W);
    }

    public static IEnumerable<Rotation3D> RotationsAroundAxis(Axis axis)
    {
        if (axis == Axis.W)
        {
            throw new ArgumentOutOfRangeException(nameof(axis), axis,
                "Axis must be a 3D spatial dimension (X, Y, or Z)");
        }
        
        for (var i = 0; i < DegreesPerRotation / NinetyDegrees; i++)
        {
            yield return new Rotation3D(axis, i * NinetyDegrees);
        }
    }

    private static Vector3D RotateAboutX(Rotation3D r, Vector3D v)
    {
        var y = v.Y * Math.Cos(r._thetaRad) - v.Z * Math.Sin(r._thetaRad);
        var z = v.Y * Math.Sin(r._thetaRad) + v.Z * Math.Cos(r._thetaRad);
        return new Vector3D(v.X, (int)Math.Round(y), (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutY(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r._thetaRad) + v.Z * Math.Sin(r._thetaRad);
        var z = v.Z * Math.Cos(r._thetaRad) - v.X * Math.Sin(r._thetaRad);
        return new Vector3D((int)Math.Round(x), v.Y, (int)Math.Round(z));
    }
    
    private static Vector3D RotateAboutZ(Rotation3D r, Vector3D v)
    {
        var x = v.X * Math.Cos(r._thetaRad) - v.Y * Math.Sin(r._thetaRad);
        var y = v.X * Math.Sin(r._thetaRad) + v.Y * Math.Cos(r._thetaRad);
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
}