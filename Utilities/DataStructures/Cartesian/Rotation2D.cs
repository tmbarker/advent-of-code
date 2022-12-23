using Utilities.Extensions;

namespace Utilities.DataStructures.Cartesian;

public readonly struct Rotation2D : IEquatable<Rotation2D>
{
    private const int DegreesPerRotation = 360;
    private const int NinetyDegrees = DegreesPerRotation / 4;
    private const string ThetaOutOfRangeError = "Theta must be an integral multiple of 90 degrees";
    
    public static readonly Rotation2D Zero = new(0);
    public static readonly Rotation2D Cw90 = new(-NinetyDegrees);
    public static readonly Rotation2D Ccw90 = new(NinetyDegrees);
    public static readonly Rotation2D Ccw180 = new(2 * NinetyDegrees);

    private int ThetaDeg { get; }
    private double ThetaRad { get; }

    private Rotation2D(int thetaDeg)
    {
        if (thetaDeg.Modulo(NinetyDegrees) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thetaDeg), thetaDeg, ThetaOutOfRangeError);
        }

        ThetaDeg = thetaDeg.Modulo(DegreesPerRotation);
        ThetaRad = ThetaDeg * Math.PI * 2 / DegreesPerRotation;
    }
    
    public static Rotation2D operator +(Rotation2D lhs, Rotation2D rhs)
    {
        return new Rotation2D(lhs.ThetaDeg + rhs.ThetaDeg);
    }
    
    public static Rotation2D operator -(Rotation2D lhs, Rotation2D rhs)
    {
        return new Rotation2D(lhs.ThetaDeg + rhs.ThetaDeg);
    }

    public static Vector2D operator *(Rotation2D lhs, Vector2D rhs)
    {
        var x = (int)(rhs.X * Math.Cos(lhs.ThetaRad) - rhs.Y * Math.Sin(lhs.ThetaRad));
        var y = (int)(rhs.X * Math.Sin(lhs.ThetaRad) + rhs.Y * Math.Cos(lhs.ThetaRad));
        return new Vector2D(x, y);
    }

    public bool Equals(Rotation2D other)
    {
        return ThetaDeg == other.ThetaDeg;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rotation2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ThetaDeg;
    }

    public static bool operator ==(Rotation2D left, Rotation2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rotation2D left, Rotation2D right)
    {
        return !left.Equals(right);
    }
}