using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D20;

public static class SeaMonster
{
    public const char Chr = '#';
    public static int Width => 20;
    public static int Height => 2;

    public static readonly HashSet<Vec2D> Pattern =
    [
        new Vec2D(x:  0, y: 1),
        new Vec2D(x:  1, y: 0),
        new Vec2D(x:  4, y: 0),
        new Vec2D(x:  5, y: 1),
        new Vec2D(x:  6, y: 1),
        new Vec2D(x:  7, y: 0),
        new Vec2D(x: 10, y: 0),
        new Vec2D(x: 11, y: 1),
        new Vec2D(x: 12, y: 1),
        new Vec2D(x: 13, y: 0),
        new Vec2D(x: 16, y: 0),
        new Vec2D(x: 17, y: 1),
        new Vec2D(x: 18, y: 1),
        new Vec2D(x: 18, y: 2),
        new Vec2D(x: 19, y: 1)
    ];
}