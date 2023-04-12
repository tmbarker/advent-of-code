using Utilities.Cartesian;

namespace Problems.Y2017.D22;

public readonly struct Pose
{
    public Vector2D Pos { get; }
    private Vector2D Face { get; }

    public Pose(Vector2D pos, Vector2D face)
    {
        Face = face;
        Pos = pos;
    }

    public Pose Step()
    {
        return new Pose(pos: Pos + Face, face: Face);
    }

    public Pose Left()
    {
        return new Pose(pos: Pos, face: Rotation3D.Positive90Z * Face);
    }

    public Pose Right()
    {
        return new Pose(pos: Pos, face: Rotation3D.Negative90Z * Face);
    }
    
    public Pose Reverse()
    {
        return new Pose(pos: Pos, face: Rotation3D.Positive180Z * Face);
    }
}