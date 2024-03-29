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
        Shape = new HashSet<Vec2D>
        {
            new(x: 0, y: 0),
            new(x: 1, y: 0),
            new(x: 2, y: 0),
            new(x: 3, y: 0)
        };
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class Plus : Rock
{
    public Plus()
    {
        Shape = new HashSet<Vec2D>
        {
            new(x: 1, y: 0),
            new(x: 1, y: 1),
            new(x: 1, y: 2),
            new(x: 0, y: 1),
            new(x: 2, y: 1)
        };
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class L : Rock
{
    public L()
    {
        Shape = new HashSet<Vec2D>
        {
            new(x: 0, y: 0),
            new(x: 1, y: 0),
            new(x: 2, y: 0),
            new(x: 2, y: 1),
            new(x: 2, y: 2)
        };
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class VerticalLine : Rock
{
    public VerticalLine()
    {
        Shape = new HashSet<Vec2D>
        {
            new(x: 0, y: 0),
            new(x: 0, y: 1),
            new(x: 0, y: 2),
            new(x: 0, y: 3)
        };
    }
    
    public override HashSet<Vec2D> Shape { get; }
}

public sealed class Square : Rock
{
    public Square()
    {
        Shape = new HashSet<Vec2D>
        {
            new(x: 0, y: 0),
            new(x: 0, y: 1),
            new(x: 1, y: 0),
            new(x: 1, y: 1)
        };
    }
    
    public override HashSet<Vec2D> Shape { get; }
}