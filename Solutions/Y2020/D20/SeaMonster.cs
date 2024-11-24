using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D20;

public static class SeaMonster
{
    public const char Chr = '#';
    public static int Width => 20;
    public static int Height => 2;

    public static readonly HashSet<Vec2D> Pattern =
    [
        new(X:  0, Y: 1),
        new(X:  1, Y: 0),
        new(X:  4, Y: 0),
        new(X:  5, Y: 1),
        new(X:  6, Y: 1),
        new(X:  7, Y: 0),
        new(X: 10, Y: 0),
        new(X: 11, Y: 1),
        new(X: 12, Y: 1),
        new(X: 13, Y: 0),
        new(X: 16, Y: 0),
        new(X: 17, Y: 1),
        new(X: 18, Y: 1),
        new(X: 18, Y: 2),
        new(X: 19, Y: 1)
    ];
}