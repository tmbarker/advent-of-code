using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D24;

public class Blizzard
{
    public Blizzard(Vector2D pos, Vector2D heading)
    {
        Pos = pos;
        Heading = heading;
    }

    public Vector2D Pos { get; set; }
    public Vector2D Heading { get; }
    public Vector2D Course => Pos + Heading;
}