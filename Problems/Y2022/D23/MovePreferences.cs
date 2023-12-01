using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D23;

public static class MovePreferences
{
    private static readonly List<(Vector2D target, HashSet<Vector2D> checkSet)> Choices = new()
    {
        (Vector2D.Up, new HashSet<Vector2D> { new(-1, 1), new(0, 1), new(1, 1) }),
        (Vector2D.Down, new HashSet<Vector2D> { new(-1, -1), new(0, -1), new(1, -1) }),
        (Vector2D.Left, new HashSet<Vector2D> { new(-1, 1), new(-1, 0), new(-1, -1) }),
        (Vector2D.Right, new HashSet<Vector2D> { new(1, 1), new(1, 0), new(1, -1) })
    };

    public static int Count => Choices.Count;

    public static (Vector2D target, HashSet<Vector2D> checkSet) Get(int i)
    {
        return Choices[i % Count];
    }
}