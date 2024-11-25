namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly value type representing a 2D pose (Position and Facing vectors).
/// </summary>
public readonly record struct Pose2D(Vec2D Pos, Vec2D Face)
{
    public Vec2D Ahead => Pos + Face;
    public Vec2D Behind => Pos - Face;
    public Vec2D Right => Pos + Rot3D.N90Z.Transform(Face);
    public Vec2D Left => Pos + Rot3D.P90Z.Transform(Face);

    public Pose2D Step()
    {
        return this with { Pos = Ahead };
    }

    public Pose2D Step(int amount)
    {
        return this with { Pos = Pos + amount * Face };
    }
    
    public Pose2D Turn(Quaternion rot)
    {
        return this with { Face = rot.Transform(Face) };
    }

    public override string ToString()
    {
        return $"{nameof(Pos)}={Pos} {nameof(Face)}={Face}";
    }
    
}