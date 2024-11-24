namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly value type representing a 2D pose (Position and Facing vectors)
/// </summary>
public readonly record struct Pose2D(Vec2D Pos, Vec2D Face)
{
    public Vec2D Ahead => Pos + Face;
    public Vec2D Behind => Pos - Face;
    public Vec2D Right => Pos + (Vec2D)(Rot3D.N90Z * Face);
    public Vec2D Left => Pos + (Vec2D)(Rot3D.P90Z * Face);

    public Pose2D Step()
    {
        return this with { Pos = Ahead };
    }

    public Pose2D Step(int amount)
    {
        return this with { Pos = Pos + amount * Face };
    }
    
    public Pose2D Turn(Rot3D rot)
    {
        if (rot.Axis != Axis.Z && rot != Rot3D.Zero)
        {
            throw new InvalidOperationException(message: $"Invalid axis of rotation [{rot.Axis}]");
        }

        return this with { Face = rot * Face };
    }

    public override string ToString()
    {
        return $"{nameof(Pos)}={Pos} {nameof(Face)}={Face}";
    }
    
}