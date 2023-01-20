using Utilities.Cartesian;

namespace Problems.Y2020.D20;

public static class SeaMonster
{
    public const char Chr = '#';
    public static int Width => 20;
    public static int Height => 2;

    public static readonly HashSet<Vector2D> Pattern = new()
    {
        new Vector2D(0, 1),
        new Vector2D(1, 0),
        new Vector2D(4, 0),
        new Vector2D(5, 1),
        new Vector2D(6, 1),
        new Vector2D(7, 0),
        new Vector2D(10, 0),
        new Vector2D(11, 1),
        new Vector2D(12, 1),
        new Vector2D(13, 0),
        new Vector2D(16, 0),
        new Vector2D(17, 1),
        new Vector2D(18, 1),
        new Vector2D(18, 2),
        new Vector2D(19, 1),
    };
}