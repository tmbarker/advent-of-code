using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D17;

public abstract class Rock
{
    public abstract HashSet<Vec2D> Shape { get; }
}

public sealed class HorizontalLine : Rock
{
    public HorizontalLine()
    {
        Shape =
        [
            new(X: 0, Y: 0),
            new(X: 1, Y: 0),
            new(X: 2, Y: 0),
            new(X: 3, Y: 0)
        ];
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class Plus : Rock
{
    public Plus()
    {
        Shape =
        [
            new(X: 1, Y: 0),
            new(X: 1, Y: 1),
            new(X: 1, Y: 2),
            new(X: 0, Y: 1),
            new(X: 2, Y: 1)
        ];
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class L : Rock
{
    public L()
    {
        Shape =
        [
            new(X: 0, Y: 0),
            new(X: 1, Y: 0),
            new(X: 2, Y: 0),
            new(X: 2, Y: 1),
            new(X: 2, Y: 2)
        ];
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class VerticalLine : Rock
{
    public VerticalLine()
    {
        Shape =
        [
            new(X: 0, Y: 0),
            new(X: 0, Y: 1),
            new(X: 0, Y: 2),
            new(X: 0, Y: 3)
        ];
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class Square : Rock
{
    public Square()
    {
        Shape =
        [
            new(X: 0, Y: 0),
            new(X: 0, Y: 1),
            new(X: 1, Y: 0),
            new(X: 1, Y: 1)
        ];
    }
    
    public override HashSet<Vec2D> Shape { get; }
}