using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D23;

public static class MovePreferences
{
    private static readonly List<(Vector2D target, HashSet<Vector2D> checkSet)> Choices =
    [
        (target: Vector2D.Up,    checkSet:[new Vector2D(x: -1, y:  1), new Vector2D(x:  0, y:  1), new Vector2D(x:  1, y:  1)]),
        (target: Vector2D.Down,  checkSet:[new Vector2D(x: -1, y: -1), new Vector2D(x:  0, y: -1), new Vector2D(x:  1, y: -1)]),
        (target: Vector2D.Left,  checkSet:[new Vector2D(x: -1, y:  1), new Vector2D(x: -1, y:  0), new Vector2D(x: -1, y: -1)]),
        (target: Vector2D.Right, checkSet:[new Vector2D(x:  1, y:  1), new Vector2D(x:  1, y:  0), new Vector2D(x:  1, y: -1)])
    ];

    public static int Count => Choices.Count;

    public static (Vector2D target, HashSet<Vector2D> checkSet) Get(int i)
    {
        return Choices[i % Count];
    }
}