using Utilities.Cartesian;

namespace Problems.Y2019.D17;

public readonly record struct Pose(Vector2D Position, Vector2D Facing);
public readonly record struct State(IReadOnlySet<Vector2D> Scaffolding, Pose Robot);

public static class Extensions
{
    public static Pose Step(this Pose pose)
    {
        return pose with { Position = pose.Position + pose.Facing };
    }
    
    public static Pose Turn(this Pose pose, Rotation3D turn)
    {
        return pose with { Facing = turn * pose.Facing };
    }
}