using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D22;

public class Pose
{
    public Pose(Vector2D pos, Vector2D facing)
    {
        Pos = pos;
        Facing = facing;
    }
    
    public Vector2D Pos { get; set; }
    public Vector2D Facing { get; set; }

    public override string ToString()
    {
        return $"Pos={Pos}  Facing={Facing}";
    }
}