using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D23;

public static class MovePreferences
{
    private static readonly List<(Vec2D target, HashSet<Vec2D> checkSet)> Choices =
    [
        (target: Vec2D.Up,    checkSet:[new Vec2D(X: -1, Y:  1), new Vec2D(X:  0, Y:  1), new Vec2D(X:  1, Y:  1)]),
        (target: Vec2D.Down,  checkSet:[new Vec2D(X: -1, Y: -1), new Vec2D(X:  0, Y: -1), new Vec2D(X:  1, Y: -1)]),
        (target: Vec2D.Left,  checkSet:[new Vec2D(X: -1, Y:  1), new Vec2D(X: -1, Y:  0), new Vec2D(X: -1, Y: -1)]),
        (target: Vec2D.Right, checkSet:[new Vec2D(X:  1, Y:  1), new Vec2D(X:  1, Y:  0), new Vec2D(X:  1, Y: -1)])
    ];

    public static int Count => Choices.Count;

    public static (Vec2D target, HashSet<Vec2D> checkSet) Get(int i)
    {
        return Choices[i % Count];
    }
}