using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D17;

public abstract class Rock
{
    public abstract HashSet<Vec2D> Shape { get; }
}

public sealed class HorizontalLine : Rock
{
    public override HashSet<Vec2D> Shape { get; } =
    [
        new(X: 0, Y: 0),
        new(X: 1, Y: 0),
        new(X: 2, Y: 0),
        new(X: 3, Y: 0)
    ];
}

public sealed class Plus : Rock
{
    public override HashSet<Vec2D> Shape { get; } =
    [
        new(X: 1, Y: 0),
        new(X: 1, Y: 1),
        new(X: 1, Y: 2),
        new(X: 0, Y: 1),
        new(X: 2, Y: 1)
    ];
}

public sealed class L : Rock
{
    public override HashSet<Vec2D> Shape { get; } =
    [
        new(X: 0, Y: 0),
        new(X: 1, Y: 0),
        new(X: 2, Y: 0),
        new(X: 2, Y: 1),
        new(X: 2, Y: 2)
    ];
}

public sealed class VerticalLine : Rock
{
    public override HashSet<Vec2D> Shape { get; } =
    [
        new(X: 0, Y: 0),
        new(X: 0, Y: 1),
        new(X: 0, Y: 2),
        new(X: 0, Y: 3)
    ];
}

public sealed class Square : Rock
{
    public override HashSet<Vec2D> Shape { get; } =
    [
        new(X: 0, Y: 0),
        new(X: 0, Y: 1),
        new(X: 1, Y: 0),
        new(X: 1, Y: 1)
    ];
}