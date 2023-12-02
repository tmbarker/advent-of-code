namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly value type representing a 2D pose (Position and Facing vectors)
/// </summary>
public readonly struct Pose2D : IEquatable<Pose2D>
{
    public Vector2D Pos { get; }
    public Vector2D Face { get; }
    public Vector2D Ahead => Pos + Face;
    
    public Pose2D(Vector2D pos, Vector2D face)
    {
        Pos = pos;
        Face = face;
    }
    
    public Pose2D Step()
    {
        return new Pose2D(pos: Ahead, face: Face);
    }
    
    public Pose2D Step(int amount)
    {
        return new Pose2D(pos: Pos + amount * Face, face: Face);
    }

    public Pose2D Turn(Rotation3D rot)
    {
        if (rot.Axis != Axis.Z && rot != Rotation3D.Zero)
        {
            throw new InvalidOperationException(message: $"Invalid axis of rotation [{rot.Axis}]");
        }

        return new Pose2D(pos: Pos, face: rot * Face);
    }
    
    public override string ToString()
    {
        return $"{nameof(Pos)}={Pos} {nameof(Face)}={Face}";
    }

    public bool Equals(Pose2D other)
    {
        return Pos.Equals(other.Pos) && Face.Equals(other.Face);
    }

    public override bool Equals(object? obj)
    {
        return obj is Pose2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pos, Face);
    }

    public static bool operator ==(Pose2D left, Pose2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Pose2D left, Pose2D right)
    {
        return !left.Equals(right);
    }
}