using Utilities.Extensions;

namespace Utilities.Cartesian;

/// <summary>
/// A readonly value type which provides integral 90 degree rotations over <see cref="Vector3D"/> instances 
/// </summary>
public readonly struct Rotation3D : IEquatable<Rotation3D>
{
    private const string ThetaOutOfRangeError = "Theta must be an integral multiple of 90 degrees";
    private const int DegreesPerRotation = 360;
    private const int NinetyDegrees = DegreesPerRotation / 4;

    private readonly Axis _axis;
    private readonly int _thetaDeg;
    private readonly double _thetaRad;

    public static readonly Rotation3D Zero = new(Axis.X, 0);
    public static readonly Rotation3D Negative90X = new(Axis.X, -NinetyDegrees);
    public static readonly Rotation3D Positive90X = new(Axis.X, NinetyDegrees);
    public static readonly Rotation3D Positive180X = new(Axis.X, 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Y = new(Axis.Y, -NinetyDegrees);
    public static readonly Rotation3D Positive90Y = new(Axis.Y, NinetyDegrees);
    public static readonly Rotation3D Positive180Y = new(Axis.Y, 2 * NinetyDegrees);
    public static readonly Rotation3D Negative90Z = new(Axis.Z, -NinetyDegrees);
    public static readonly Rotation3D Positive90Z = new(Axis.Z, NinetyDegrees);
    public static readonly Rotation3D Positive180Z = new(Axis.Z, 2 * NinetyDegrees);

    public Rotation3D(Axis axis, int thetaDeg)
    {
        if (thetaDeg.Modulo(NinetyDegrees) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thetaDeg), thetaDeg, ThetaOutOfRangeError);
        }

        _axis = axis;
        _thetaDeg = thetaDeg.Modulo(DegreesPerRotation);
        _thetaRad = thetaDeg * Math.PI * 2 / DegreesPerRotation;
    }

    public static Vector3D operator *(Rotation3D r, Vector3D v)
    {
        return r._axis switch
        {
            Axis.X => RotateAboutX(r, v),
            Axis.Y => RotateAboutY(r, v),
            Axis.Z => RotateAboutZ(r, v),
            _ => throw new InvalidOperationException($"Invalid axis of rotation [{r._axis}]"),
        };
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
        return _thetaDeg == other._thetaDeg && _axis == other._axis;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rotation3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_axis, _thetaDeg);
    }

    public override string ToString()
    {
        return $"R({_axis}): {_thetaDeg}°";
    }

    public enum Axis
    {
        X,
        Y,
        Z,
    }
}