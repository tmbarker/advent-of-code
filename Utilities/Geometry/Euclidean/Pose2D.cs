namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly value type representing a 2D pose (Position and Facing vectors)
/// </summary>
public readonly struct Pose2D(Vec2D pos, Vec2D face) : IEquatable<Pose2D>
{
    public Vec2D Pos { get; } = pos;
    public Vec2D Face { get; } = face;
    public Vec2D Ahead => Pos + Face;
    public Vec2D Behind => Pos - Face;
    public Vec2D Right => GetSideAdjacent(right: true);
    public Vec2D Left => GetSideAdjacent(right: false);

    public Pose2D Step()
    {
        return new Pose2D(pos: Ahead, face: Face);
    }
    
    public Pose2D Step(int amount)
    {
        return new Pose2D(pos: Pos + amount * Face, face: Face);
    }

    public Vec2D GetSideAdjacent(bool right)
    {
        var dir = right
            ? Rot3D.N90Z * Face
            : Rot3D.P90Z * Face;

        return Pos + (Vec2D)dir;
    }
    
    public Pose2D Turn(Rot3D rot)
    {
        if (rot.Axis != Axis.Z && rot != Rot3D.Zero)
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